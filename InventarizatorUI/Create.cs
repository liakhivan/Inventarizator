using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories;

namespace InventarizatorUI
{
    public partial class Create : Form
    {
        public Create()
        {
            InitializeComponent();
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    ProductRepository repos = new ProductRepository();
                    Product product = new Product(textBox1.Text);
                    repos.Create(new Product(textBox1.Text));
                    CreateRecept form3 = new CreateRecept(product);
                }
                else
                {
                    IngredientRepository repos = new IngredientRepository();
                    repos.Create(new Ingredient(textBox1.Text));
                }
                label2.Text = "Об'єкт було успішно створено.";
            }
            catch (ArgumentException)
            {
                label2.Text = "Цей об'єкт вже існує.";
            }
        }
    }
}
