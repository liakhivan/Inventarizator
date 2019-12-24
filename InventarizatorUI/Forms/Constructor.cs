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
        public Constructor()
        {
            InitializeComponent();
            invoice = new List<ElementOfInvoice>();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            try
            {
                var nameSomeProduct = comboBox1.SelectedItem.ToString();
                var weightSomeProduct = Double.Parse(comboBox2.SelectedItem.ToString());
                var countSomeProduct = Decimal.ToInt32(numericUpDown1.Value);
                var priceSomeProduct = Double.Parse(textBox1.Text);

                invoice.Add(new ElementOfInvoice(nameSomeProduct, weightSomeProduct, countSomeProduct, priceSomeProduct));

                listBox1.Items.Add(nameSomeProduct + "     " + weightSomeProduct.ToString("0.00") + "     " +
                    countSomeProduct + "     " + priceSomeProduct);

                create.Enabled = true;
            }
            catch (Exception)
            {
                label5.ForeColor = System.Drawing.Color.Red;
                label5.Text = @"Введено некоректні дані.";
            }

        }

        private void Create_Click(object sender, EventArgs e)
        {
            if ((invoice.Count != 0) && (textBox2.Text.Length != 0) && (textBox4.Text.Length != 0))
            {
                Excel.Application invoiceFile = new Excel.Application
                {
                    Visible = true
                };
                try
                {
                    if (!Int32.TryParse(textBox2.Text, out int res))
                        throw new ArgumentException("№ накладної повинен бути числом!!!");
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

                    //Заповнення накладної.
                    sheet.Range["H17"].Value = textBox3.Text;
                    sheet.Range["AJ5"].Value = textBox2.Text;

                    int i = 0;
                    foreach (var element in invoice)
                    {
                        // №
                        sheet.Range["A"+ (startProductPosition + i)].Value = i + 1;
                        // Назва
                        sheet.Range["E" + (startProductPosition + i)].Value = element.Product;
                        // Одиниці виміру
                        sheet.Range["AD" + (startProductPosition + i)].Value = "кг";
                        // Кількість
                        sheet.Range["AJ" + (startProductPosition + i)].Value = element.Count * element.Weight;
                        // Ціна
                        sheet.Range["AP" + (startProductPosition + i)].Value = element.Price;
                        i++;
                    }
                    invoiceFile.ActiveWorkbook.Save();

                    label5.ForeColor = System.Drawing.Color.Green;
                    label5.Text = @"Накладна успішно створена.";
                }
                catch (Exception ex)
                {
                    label5.ForeColor = System.Drawing.Color.Red;
                    label5.Text = ex.Message;
                }
                finally
                {
                    invoiceFile.Workbooks.Close();
                    invoiceFile.Quit();
                }
            }
            else
            {
                label5.ForeColor = System.Drawing.Color.Red;
                label5.Text = @"Не всі поля заповнені.";
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            invoice.RemoveAt(listBox1.SelectedIndex);
            listBox1.Items.Clear();

            foreach (var n in invoice)
                listBox1.Items.Add(n.Product + "     " + n.Weight.ToString("0.00") + "     " + n.Count + "     " + n.Price);

            if(listBox1.Items.Count == 0)
                delete.Enabled = deleteAll.Enabled = create.Enabled = false;
        }

        private void DeleteAll_Click(object sender, EventArgs e)
        {
            invoice.Clear();
            listBox1.Items.Clear();
            
            delete.Enabled = deleteAll.Enabled = create.Enabled = false;
        }

        private void Constructor_MouseMove(object sender, MouseEventArgs e)
        {

            label5.ForeColor = System.Drawing.Color.Black;
            label5.Text = @"";
        }

        private void Constructor_Load(object sender, EventArgs e)
        {
            ProductRepository productRepository = new ProductRepository();
            comboBox1.DataSource = productRepository.GetDataSource();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductRepository productRepository = new ProductRepository();
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
    }
}
