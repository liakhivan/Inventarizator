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
        private event Delete.Upd updateInformation;

        public DeleteMessage(Ingredient entryIngredient, Delete.Upd updateEvent)
        {
            updateInformation += updateEvent;
            this.entryIngredient = entryIngredient;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IngredientRepository ingredientRepository = new IngredientRepository();
            IngredientsForProductRepository ingredientsForProductRepository = new IngredientsForProductRepository();
            ProductRepository productRepository = new ProductRepository();
            Ingredient ingredient = ingredientRepository.GetDataSource().First(element => element.Name == entryIngredient.Name);

            ingredientRepository.Delete(ingredient);

            updateInformation();
            MessageBox.Show(@"Видалення успішне.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
