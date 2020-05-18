using InventarizatorLI.Model;
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
                    var repos = new IngredientRepository();
                    var ingredientsForProductRepository = new IngredientsForProductRepository();
                    Ingredient ingredient;
                    IngredientsForProduct ingredientForProduct = null;
                    do
                    {
                        ingredient = repos.GetDataSource().First(element => element.Name == listBox1.SelectedItem.ToString());
                        ingredientForProduct = ingredientsForProductRepository.GetDataSource().
                            FirstOrDefault(element => element.IngredientId == ingredient.Id);
                        if (ingredientForProduct != null)
                        {
                            ProductRepository productRepository = new ProductRepository();
                            var product = productRepository.GetDataSource().First(element => element.Id == ingredientForProduct.ProductId);
                            productRepository.Delete(product);
                        }
                    } while (ingredientForProduct != null);
                    repos.Delete(ingredient);
                    listBox1.DataSource = repos.GetDataSource();
                }
                MessageBox.Show(@"Видалення успішне.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updateInformation();
            }
            catch(InvalidOperationException exeption)
            {
                MessageBox.Show(exeption.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
