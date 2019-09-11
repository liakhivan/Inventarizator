using System;
using System.Windows.Forms;
using InventarizatorLI.Repositories;

namespace InventarizatorUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ProductRepository repository = new ProductRepository();
            dataGridView1.DataSource = repository.GetProductConteinerDataSource();
        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            IngredientRepository repos = new IngredientRepository();
            dataGridView1.DataSource = repos.GetIngredientPackageDataSource();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ProductRepository repository = new ProductRepository();
            dataGridView1.DataSource = repository.GetProductConteinerDataSource();
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
    }
}
