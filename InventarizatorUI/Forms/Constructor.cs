using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using InventarizatorLI.Model;
using System.Collections.Generic;
using InventarizatorLI.Repositories;
using Excel = Microsoft.Office.Interop.Excel;

namespace InventarizatorUI.Forms
{
    public partial class Constructor : Form
    {
        private const string currentFileName = @"\invoice.xlsx";
        private const int startProductPosition = 30;
        private string path;
        private string newFileName;
        private List<ElementOfInvoice> invoice;

        private BindingSource bsProductCollection;
        private List<string> productCollection;

        public Constructor()
        {
            InitializeComponent();
            invoice = new List<ElementOfInvoice>();

            ProductRepository productRepository = new ProductRepository();

            productCollection = productRepository.GetDataSource().Select(n => n.ToString()).ToList();
            bsProductCollection = new BindingSource();
            bsProductCollection.DataSource = productCollection;
            comboBox1.DataSource = bsProductCollection;
            comboBox1.SelectedItem = null;

            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            comboBox1.AutoCompleteMode = AutoCompleteMode.None;
            comboBox1.IntegralHeight = false;
        }

        private string Mounth(int number)
        {
            string mounth = "";
            switch (number)
            {
                case 1:
                    mounth = "січня";
                    break;
                case 2:
                    mounth = "лютого";
                    break;
                case 3:
                    mounth = "березня";
                    break;
                case 4:
                    mounth = "квітня";
                    break;
                case 5:
                    mounth = "травня";
                    break;
                case 6:
                    mounth = "червня";
                    break;
                case 7:
                    mounth = "липня";
                    break;
                case 8:
                    mounth = "серпня";
                    break;
                case 9:
                    mounth = "вересня";
                    break;
                case 10:
                    mounth = "жовтня";
                    break;
                case 11:
                    mounth = "листопада";
                    break;
                case 12:
                    mounth = "грудня";
                    break;
                default:
                    break;
            }
            return mounth;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            try
            {
                var nameSomeProduct = comboBox1.SelectedItem.ToString();
                var weightSomeProduct = Double.Parse(comboBox2.Text.ToString());
                var countSomeProduct = Decimal.ToInt32(numericUpDown1.Value);
                var priceSomeProduct = Double.Parse(textBox1.Text);
                ElementOfInvoice element = new ElementOfInvoice(nameSomeProduct, weightSomeProduct, countSomeProduct, priceSomeProduct, (radioButton1.Checked) ? "кг" : "шт");
                //var currProductInInvoice = invoice.Where(elemInvoice => (elemInvoice.Product == element.Product)).ToList();
                var currProductInInvoice = invoice.Where(n => n.UnitOfMeasurement == element.UnitOfMeasurement & n.Product == element.Product).ToList();
                if (currProductInInvoice.Count != 0)
                {
                    if(element.UnitOfMeasurement == "шт")
                        if(currProductInInvoice[0].Price != priceSomeProduct & currProductInInvoice[0].Weight == element.Weight)
                            throw new ArgumentException("Ціна даного продукту не співпадає з наявним продуктом.");

                    if (currProductInInvoice[0].Price != priceSomeProduct)
                        throw new ArgumentException("Ціна даного продукту не співпадає з наявним продуктом.");
                }

                var currProductsInInvoice = invoice.Where(elemInvoice => (elemInvoice.Product == element.Product) & (elemInvoice.Weight == element.Weight)).ToList();
                if (currProductsInInvoice != null)
                {
                    int countOfCurrProductContainers = currProductsInInvoice.Sum(n => n.Count);

                    if (countOfCurrProductContainers + element.Count > Int32.Parse(label5.Text))
                        throw new ArgumentOutOfRangeException("Загальна кількість продукту з даною вагою перевищує допустиму.");

                    //foreach (var item in invoice)
                    //{
                    //    if (item.Product == element.Product & item.Weight == element.Weight)
                    //    {
                    //        if (item.Count + element.Count > Int32.Parse(label5.Text))
                    //            throw new ArgumentOutOfRangeException("Загальна кількість продукту з даною вагою перевищує допустиму.");
                    //    }
                    //}
                }
                if ((invoice.FirstOrDefault(elemInvoice => (
                    elemInvoice.Product == element.Product)
                    & (elemInvoice.Weight == element.Weight)
                    & (elemInvoice.UnitOfMeasurement == element.UnitOfMeasurement)
                    & (elemInvoice.Price == element.Price)
                )) != null)
                {
                    listBox1.Items.Clear();
                    foreach (var item in invoice)
                    {
                        if (item.Product == element.Product & item.Weight == element.Weight & item.UnitOfMeasurement == element.UnitOfMeasurement)
                        {
                            item.Count += element.Count;
                        }
                        listBox1.Items.Add($"{item.Product}  {item.Weight:f2} кг.  {item.Count} шт.   {item.Price} грн.      (од. вим. {item.UnitOfMeasurement})");
                    }
                }
                else
                {
                    invoice.Add(element);
                    listBox1.Items.Add($"{nameSomeProduct}  {weightSomeProduct:f2} кг.  {countSomeProduct} шт.  {priceSomeProduct} грн.      (од. вим. {element.UnitOfMeasurement})");
                }
                create.Enabled = true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(@"Введено некоректні дані.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Create_Click(object sender, EventArgs e)
        {
            if ((invoice.Count != 0) && (textBox2.Text.Length != 0) && (textBox4.Text.Length != 0))
            {
                Excel.Application invoiceFile = new Excel.Application
                {
                    Visible = false
                };
                try
                {
                    // Створення нового списку продуктів.
                    List<ElementOfInvoice> newInvoice = new List<ElementOfInvoice>();

                    // Спрощення старого списку продуктів та його уніфікація.
                    for (int index = 0; index < invoice.Count(); index++)
                    {
                        // Знаходження єдиної назви.
                        string currProductName = invoice[index].Product;
                        string currUnitOfMeasuring = invoice[index].UnitOfMeasurement;
                        double currWeight = invoice[index].Weight;
                        // Якщо така назва продукту інує в новому списку і одиниці вимірювання - кг, то пропустити цей продукт.
                        if (newInvoice.FirstOrDefault(n => n.Product == currProductName & n.UnitOfMeasurement == currUnitOfMeasuring) != null)
                        {
                            continue;
                        }


                        if (currUnitOfMeasuring == "кг")
                        {
                            // Знайти всі продукти з такою назвою і одиницею вимірювання.
                            var elementsInvoice = invoice.Where(n => n.Product == currProductName && n.UnitOfMeasurement == currUnitOfMeasuring).ToList();
                            // Знайти єдину вагу, кількість продукту та ціну.
                            double weight = elementsInvoice.Sum(n => n.Weight * n.Count);
                            int count = 1;
                            double price = invoice[index].Price;
                            newInvoice.Add(new ElementOfInvoice(currProductName, weight, count, price, "кг"));
                        }

                        if (currUnitOfMeasuring == "шт")
                        {
                            // Знайти всі продукти з такою назвою і одиницею вимірювання.
                            var elementsInvoice = invoice.Where(n => n.Product == currProductName & n.UnitOfMeasurement == currUnitOfMeasuring & n.Weight == currWeight).ToList();
                            // Знайти єдину вагу, кількість продукту та ціну.
                            double weight = elementsInvoice[0].Weight;
                            int count = elementsInvoice.Sum(n => n.Count);
                            double price = invoice[index].Price;
                            newInvoice.Add(new ElementOfInvoice(currProductName, weight, count, price, "шт"));
                        }
                    }

                    if (!Int32.TryParse(textBox2.Text, out int res))
                    {
                        throw new ArgumentException("№ накладної повинен бути числом!!!");
                    }
                    // Генерація імені накладної.
                    newFileName = textBox3.Text + "_" + textBox2.Text + "_" + dateTimePicker1.Value.ToString("dd-MM-yyyy") + ".xlsx";
                    // Копіювання файлу з корінної папки в потрібну.
                    FileInfo currFileInfo = new FileInfo(Directory.GetCurrentDirectory() + currentFileName);
                    currFileInfo.CopyTo(path + currentFileName, true);
                    // Перейменування скопійованого файлу.
                    FileInfo invoiceFileInfo = new FileInfo(path + currentFileName);
                    invoiceFileInfo.MoveTo(path + "\\" + newFileName);
                    // Відкриття файлу накладної.
                    invoiceFile.Workbooks.Open(path + "\\" + newFileName);

                    Excel.Worksheet sheet = invoiceFile.Sheets[1];
                    Excel.Worksheet sheetRemove = invoiceFile.Sheets[2];

                    //Заповнення накладної.
                    sheet.Range["H17"].Value = textBox3.Text;
                    sheet.Range["AJ5"].Value = textBox2.Text;
                    sheet.Range["AJ10"].Value = dateTimePicker1.Value.Day;
                    sheet.Range["AM10"].Value = Mounth(dateTimePicker1.Value.Month) + " " + dateTimePicker1.Value.Year;

                    int i = 0;
                    foreach (var element in newInvoice)
                    {
                        // №
                        sheet.Range["A"+ (startProductPosition + i)].Value = i + 1;
                        // Назва
                        sheet.Range["E" + (startProductPosition + i)].Value = element.Product + " " + ((element.UnitOfMeasurement == "шт") ? element.Weight.ToString() :"");
                        // Одиниці виміру
                        sheet.Range["AD" + (startProductPosition + i)].Value = element.UnitOfMeasurement;
                        // Кількість
                        sheet.Range["AJ" + (startProductPosition + i)].Value = (element.UnitOfMeasurement == "кг")? (element.Count * element.Weight) : element.Count;
                        // Ціна
                        sheet.Range["AP" + (startProductPosition + i)].Value = element.Price;
                        i++;
                    }

                    i = 1;
                    foreach (var element in invoice)
                    {
                        sheetRemove.Range["A" + i].Value = element.Product;
                        sheetRemove.Range["B" + i].Value = element.Count;
                        sheetRemove.Range["C" + i].Value = element.Weight;
                        sheetRemove.Range["D" + i].Value = element.UnitOfMeasurement;
                        i++;
                    }
                    sheetRemove.Range["E1"].Value = dateTimePicker1.Value.ToString("dd.MM.yyyy");

                    invoiceFile.ActiveWorkbook.Save();

                    MessageBox.Show( @"Накладна успішно створена.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    invoiceFile.Workbooks.Close();
                    invoiceFile.Quit();
                }
            }
            else
            {
                MessageBox.Show(@"Не всі поля заповнені.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            invoice.RemoveAt(listBox1.SelectedIndex);
            listBox1.Items.Clear();

            foreach (var n in invoice)
                listBox1.Items.Add($"{n.Product}  {n.Weight:f2} кг.  {n.Count} шт.   {n.Price} грн.      (од. вим. {n.UnitOfMeasurement})");

            if (listBox1.Items.Count == 0)
                delete.Enabled = deleteAll.Enabled = create.Enabled = false;
        }

        private void DeleteAll_Click(object sender, EventArgs e)
        {
            invoice.Clear();
            listBox1.Items.Clear();
            
            delete.Enabled = deleteAll.Enabled = create.Enabled = false;
        }

        private void Constructor_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductRepository productRepository = new ProductRepository();
            if (comboBox1.SelectedItem == null)
                return;
            comboBox2.DataSource = productRepository.GetProductConteinerDataSource().
                Where(element => element.Name == comboBox1.SelectedValue.ToString()).
                Select(n => n.Weight.ToString()).ToList();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            add.Enabled = textBox1.Text != "";
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            delete.Enabled = deleteAll.Enabled = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                textBox4.Text = path = fbd.SelectedPath;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductRepository productRepository = new ProductRepository();
            numericUpDown1.Maximum = productRepository.GetProductConteinerDataSource().First(n => n.Name == comboBox1.SelectedItem.ToString() & n.Weight == Double.Parse(comboBox2.SelectedItem.ToString())).Amount;
            numericUpDown1.Value = 1;
            label5.Text = numericUpDown1.Maximum.ToString();

            textBox1.Focus();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar) || e.KeyChar.Equals(',')) 
                return;
            
            e.Handled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                radioButton2.Checked = false;


        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                radioButton1.Checked = false;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar))
                return;

            e.Handled = true;
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            string searchString = comboBox1.Text;
            Cursor prevCursor = this.Cursor;
            bsProductCollection.DataSource = productCollection.Where(x => x.ToUpper().Contains(searchString.ToUpper())).ToList();
            if (bsProductCollection.Count != 0)
                comboBox1.DroppedDown = false;
            comboBox1.DroppedDown = true;
            comboBox1.SelectedItem = null;

            comboBox1.Text = searchString;

            // Перенесення курсора в кінець поля вводу.
            comboBox1.Select(searchString.Length, 0);

            this.Cursor = prevCursor;
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comboBox2.Focus();
            }
        }
    }
}
