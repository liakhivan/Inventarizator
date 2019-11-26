using InventarizatorLI.Model;
using InventarizatorLI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace InventarizatorUI.Forms
{
    public partial class Constructor : Form
    {
        private List<ElementOfInvoice> invoice = new List<ElementOfInvoice>();
        public Constructor()
        {
            InitializeComponent();
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
            if(invoice.Count != 0)
            {
                
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
            label5.Text = @"Інформація.";
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
    }
}
