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
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            UpdateDataGridWiew();
            comboBox1.SelectedIndex = 0;
            dataGridView1.Columns["Weight"].DefaultCellStyle.Format = "#0.00";
            radioButton1.Checked = true;
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
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            IngredientRepository repos = new IngredientRepository();
            dataGridView1.DataSource = repos.GetIngredientPackageDataSource();
            dataGridView1.AutoResizeColumns();
            panel1.Visible = false;
            maskedTextBox1.Text = " ,";
            dataGridView1.Columns["Weight"].DefaultCellStyle.Format = "#0.000";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ProductRepository repository = new ProductRepository();
            dataGridView1.DataSource = repository.GetProductConteinerDataSource();
            dataGridView1.AutoResizeColumns();
            panel1.Visible = true;
            comboBox1.SelectedIndex = 0;
            dataGridView1.Columns["Weight"].DefaultCellStyle.Format = "#0.00";
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

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add form3 = new Add(UpdateDataGridWiew);
            form3.ShowDialog();
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

        private void EliminationToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var form5 = new Remove(UpdateDataGridWiew);
            form5.ShowDialog();
        }

        public void UpdateDataGridWiew()
        {
            if(radioButton1.Checked)
            {
                ProductRepository repository = new ProductRepository();
                dataGridView1.DataSource = repository.GetProductConteinerDataSource();
                dataGridView1.AutoResizeColumns();
            }
            else
            {
                IngredientRepository repos = new IngredientRepository();
                dataGridView1.DataSource = repos.GetIngredientPackageDataSource();
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
    }
}
