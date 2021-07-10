using System;
using System.Linq;
using System.Windows.Forms;
using InventarizatorLI.Model;
using System.Collections.Generic;
using InventarizatorLI.Repositories;
using Excel = Microsoft.Office.Interop.Excel;

namespace InventarizatorUI.Forms
{
    public partial class ParserInvoice : Form
    {
        public delegate void Upd();
        private event Upd updateInformation;
        Dictionary<Conteiner, int> productCount;
        DateTime date;
        private const int startProductPosition = 30;
        Excel.Worksheet sheet;
        Excel.Application invoiceFile = new Excel.Application
        {
            Visible = false
        };

        public ParserInvoice(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            InitializeComponent();
            productCount = new Dictionary<Conteiner, int>();
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                string nameProduct;
                int count;
                double weight;

                productCount.Clear();
                textBox1.Text = "";

                ConteinerRepository conteinerRepository = new ConteinerRepository();
                ProductRepository productRepository = new ProductRepository();

                OpenFileDialog OPF = new OpenFileDialog();
                OPF.Filter = "MS Excel files|*.xlsx";
                OPF.Title = "Excel invoice";
                if (OPF.ShowDialog() == DialogResult.OK)
                {
                    string fileInvoice = OPF.FileName;
                    // Відкриття файлу накладної.
                    invoiceFile.Workbooks.Open(fileInvoice.ToString());
                }

                sheet = invoiceFile.Sheets[2];

                string dateString = sheet.Range["E1"].Value;
                var massDate = dateString.Split('.');

                date = new DateTime(Int32.Parse(massDate[2]), Int32.Parse(massDate[1]), Int32.Parse(massDate[0]));
                int i = 1;
                while ((string)(sheet.Range["A" + i].Value) != null)
                {
                    nameProduct = sheet.Range["A" + i].Value;
                    count = (int)sheet.Range["B" + i].Value;
                    weight = sheet.Range["C" + i].Value;
                    textBox1.Text += nameProduct + "   " + count + " шт. по " + String.Format("{0:f2}", weight) + " кг.\r\n";
                    var product = productRepository.GetDataSource().FirstOrDefault(n => n.Name == nameProduct);
                    if (product == null)
                    {
                        throw new ArgumentException();
                    }
                    var conteiner = conteinerRepository.GetDataSource().FirstOrDefault(n => (n.ProductId == product.Id & n.Weight == weight));
                    if (conteiner == null)
                    {
                        throw new ArgumentNullException("");
                    }
                    productCount.Add(conteiner, count);
                    i++;
                }
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

        private void button2_Click(object sender, EventArgs e)
        {
            ConteinerRepository conteinerRepository = new ConteinerRepository();
            ProductRepository productRepository = new ProductRepository();

            try
            {
                foreach(var elem in productCount)
                {
                    conteinerRepository.Remove(elem.Key.Id, date, 2, elem.Value);
                }

                MessageBox.Show(@"Продукти успішно списані.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                updateInformation();
            }
        }
    }
}
