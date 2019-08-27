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
    public partial class CreateRecept : Form
    {
        List<IngredientsForProduct> recept = new List<IngredientsForProduct>();
        Product product;
        IngredientRepository ingredientRepository = new IngredientRepository();
        BindingList<Ingredient> dataSource;
        Form form;
        public CreateRecept(Product product)
        {
            InitializeComponent();
            comboBox1.DataSource = dataSource = ingredientRepository.GetDataSource();
            this.product = product;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex < 0)
                    throw new ArgumentException();
                else
                {
                    label1.ForeColor = Color.Black;
                    recept.Add(new IngredientsForProduct(product, dataSource[comboBox1.SelectedIndex], Double.Parse(maskedTextBox1.Text)));
                }
            } catch(ArgumentException)
            {
                label1.ForeColor = Color.Red;
            }
        }

        private void CreateRecept_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //form.
        }
    }
}
