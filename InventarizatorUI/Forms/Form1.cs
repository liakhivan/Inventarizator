using System;
using System.Windows.Forms;
using InventarizatorLI.Repositories;
using System.Linq;
using InventarizatorUI.Forms;

namespace InventarizatorUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Filter()
        {
            if (radioButton1.Checked)
            {
                var Products = new ProductRepository();
                var dataSource = Products.GetProductConteinerDataSource();
                if (textBox1.Text != "")
                    dataSource = dataSource.Where(product => product.Name.ToUpper().Contains(textBox1.Text.ToUpper())).ToList();
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                ProductRepository repository = new ProductRepository();
                UpdateDataGridWiew();
                panel1.Visible = true;
                comboBox1.SelectedIndex = 0;
                dataGridView1.Columns["Weight"].DefaultCellStyle.Format = "#0.00";

                dataGridView1.Columns[0].HeaderText = @"Назва";
                dataGridView1.Columns[1].HeaderText = @"Вага";
                dataGridView1.Columns[2].HeaderText = @"Кількість";
            }
            else
            {
                IngredientRepository repos = new IngredientRepository();
                UpdateDataGridWiew();
                panel1.Visible = false;
                maskedTextBox1.Text = " ,";
                dataGridView1.Columns["Weight"].DefaultCellStyle.Format = "#0.000";

                dataGridView1.Columns[0].HeaderText = @"Назва";
                dataGridView1.Columns[1].HeaderText = @"Вага";
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Create form2 = new Create();
            form2.ShowDialog();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete form4 = new Delete(UpdateDataGridWiew);
            form4.ShowDialog();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        public void UpdateDataGridWiew()
        {
            if(radioButton1.Checked)
            {
                ProductRepository repository = new ProductRepository();
                dataGridView1.DataSource = repository.GetProductConteinerDataSource().OrderBy(n => n.Name).ToList();
                dataGridView1.AutoResizeColumns();
            }
            else
            {
                IngredientRepository repos = new IngredientRepository();
                dataGridView1.DataSource = repos.GetIngredientPackageDataSource().OrderBy(n => n.Name).ToList();
                dataGridView1.AutoResizeColumns();
            }
        }

        private void BackupToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var form6 = new Backup();
            form6.ShowDialog();
        }

        private void RecoveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form7 = new Restore(UpdateDataGridWiew);
            form7.ShowDialog();
        }

        private void MaskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void ShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.ShowDialog();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearStatistics clearStatistics = new ClearStatistics();
            clearStatistics.ShowDialog();
        }

        private void InvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParserInvoice parserInvoice = new ParserInvoice(UpdateDataGridWiew);
            parserInvoice.ShowDialog();
        }

        private void ManyallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form5 = new Remove(UpdateDataGridWiew);
            form5.ShowDialog();
        }

        private void CreateInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form5 = new Constructor();
            form5.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            UpdateDataGridWiew();
            comboBox1.SelectedIndex = 0;
            dataGridView1.Columns["Weight"].DefaultCellStyle.Format = "#0.00";
            dataGridView1.RowHeadersVisible = false;
            radioButton1.Checked = true;
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Edit edit = new Edit();
            edit.ShowDialog();
        }

        private void FormatingDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductRepository.FormatingAllData();
        }

        private void addProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var form = new AddProducts(UpdateDataGridWiew);
            form.ShowDialog();
        }

        private void addIngredientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AddIngradients(UpdateDataGridWiew);
            form.ShowDialog();
        }
    }
}
