using System;
using System.Windows.Forms;
using InventarizatorLI.Repositories;
using System.Linq;

namespace InventarizatorUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ProductRepository repository = new ProductRepository();
            dataGridView1.DataSource = repository.GetProductConteinerDataSource();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            comboBox1.SelectedIndex = 0;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            IngredientRepository repos = new IngredientRepository();
            dataGridView1.DataSource = repos.GetIngredientPackageDataSource();
            dataGridView1.AutoResizeColumns();
            panel2.Visible = false;
            maskedTextBox1.Text = " ,";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ProductRepository repository = new ProductRepository();
            dataGridView1.DataSource = repository.GetProductConteinerDataSource();
            dataGridView1.AutoResizeColumns();
            panel2.Visible = true;
            comboBox1.SelectedIndex = 0;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Create form2 = new Create();
            form2.Show();
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add form3 = new Add();
            form3.Show();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete form4 = new Delete();
            form4.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                var Products = new ProductRepository();
                var dataSource = Products.GetProductConteinerDataSource();
                if (textBox1.Text != "")
                    dataSource = dataSource.Where(product => product.Name.Contains(textBox1.Text)).ToList();
                if (maskedTextBox1.Text != " ,")
                    switch (comboBox1.SelectedIndex)
                    {
                        case 0:
                            dataSource = dataSource.Where(product => product.Weight == Double.Parse(maskedTextBox1.Text)).ToList();
                            break;
                        case 1:
                            dataSource = dataSource.Where(product => product.Weight > Double.Parse(maskedTextBox1.Text)).ToList();
                            break;
                        case 2:
                            dataSource = dataSource.Where(product => product.Weight < Double.Parse(maskedTextBox1.Text)).ToList();
                            break;
                        default:
                            break;
                    }
                dataGridView1.DataSource = dataSource;
            }
            else
            {
                var Ingredients = new IngredientRepository();
                var dataSource = Ingredients.GetIngredientPackageDataSource();
                if (textBox1.Text != "")
                    dataSource = dataSource.Where(ingredient => ingredient.Name.Contains(textBox1.Text)).ToList();
                dataGridView1.DataSource = dataSource;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Button1_Click(sender, e);
        }

        private void EliminationToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var form5 = new Remove();
            form5.Show();
        }
    }
}
