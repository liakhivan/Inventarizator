using InventarizatorLI.Repositories;
using System;
using System.Linq;
using System.Windows.Forms;

namespace InventarizatorUI
{
    public partial class Delete : Form
    {
        public delegate void Upd();
        private event Upd updateInformation;
        public Delete(Upd updateEvent)
        {
            updateInformation += updateEvent;
            InitializeComponent();
            ProductRepository repos = new ProductRepository();
            listBox1.DataSource = repos.GetDataSource();
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                ProductRepository repos = new ProductRepository();
                listBox1.DataSource = repos.GetDataSource();
            }
            else
            {
                IngredientRepository repos = new IngredientRepository();
                listBox1.DataSource = repos.GetDataSource();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    ProductRepository repos = new ProductRepository();
                    var product = repos.GetDataSource().First(element => element.Name == listBox1.SelectedItem.ToString());
                    repos.Delete(product);
                    listBox1.DataSource = repos.GetDataSource();
                }
                else
                {
                    IngredientRepository repos = new IngredientRepository();
                    var ingredient = repos.GetDataSource().First(element => element.Name == listBox1.SelectedItem.ToString());
                    repos.Delete(ingredient);
                    listBox1.DataSource = repos.GetDataSource();
                }
                label1.ForeColor = System.Drawing.Color.Green;
                label1.Text = @"Видалення успішне.";
                updateInformation();
            }
            catch(InvalidOperationException exeption)
            {
                label1.ForeColor = System.Drawing.Color.Red;
                label1.Text = exeption.Message;
            }
        }

        private void Delete_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = @"Інформація про видалення.";
            label1.ForeColor = System.Drawing.Color.Black;
        }
    }
}
