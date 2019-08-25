using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories;

namespace InventarizatorUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            IngredientRepository ingredient = new IngredientRepository();
        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            //dataGridView1.Columns.Clear();
            //dataGridView1.Columns.Add(new DataGridViewColumn() { HeaderText = "Назва", Width = 120 });
            //dataGridView1.Columns.Add(new DataGridViewColumn() { HeaderText = "Вага", Width = 50 });
            IngredientRepository repos = new IngredientRepository();
            dataGridView1.DataSource = repos.GetIngredientPackageDataSource();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //dataGridView1.Columns.Clear();
            //dataGridView1.Columns.Add(new DataGridViewColumn() { HeaderText = "Назва", Width = 120 });
            //dataGridView1.Columns.Add(new DataGridViewColumn() { HeaderText = "Вага", Width = 50 });
            //dataGridView1.Columns.Add(new DataGridViewColumn() { HeaderText = "Кількість", Width = 80 });
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
    }
}
