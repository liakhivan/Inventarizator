using InventarizatorLI.Model;
using InventarizatorLI.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventarizatorUI.Forms
{
    public partial class DeleteMessage : Form
    {
        private Ingredient entryIngredient;
        public DeleteMessage(Ingredient entryIngredient)
        {
            this.entryIngredient = entryIngredient;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IngredientRepository repos = new IngredientRepository();
            IngredientsForProductRepository ingredientsForProductRepository = new IngredientsForProductRepository();

            ingredientsForProductRepository.GetDataSource().
                Where(element => element.IngredientId == entryIngredient.Id).ToList().ForEach(n => n.IngredientId = entryIngredient.Id);
            

            //do
            //{

            //    if (ingredientForProduct != null)
            //    {
            //        ProductRepository productRepository = new ProductRepository();
            //        var product = productRepository.GetDataSource().First(element => element.Id == ingredientForProduct.ProductId);
            //        productRepository.Delete(product);
            //    }
            //} while (ingredientForProduct != null);
            //repos.Delete(ingredient);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeWithDeleting changeWithDeleting = new ChangeWithDeleting(entryIngredient);
            changeWithDeleting.ShowDialog();

            IngredientRepository repos = new IngredientRepository();
            IngredientsForProductRepository ingredientsForProductRepository = new IngredientsForProductRepository();

            ingredientsForProductRepository.GetDataSource().
                Where(element => element.IngredientId == entryIngredient.Id).ToList().ForEach(n => n.IngredientId = entryIngredient.Id);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
