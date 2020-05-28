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
    public partial class ChangeWithDeleting : Form
    {
        private Ingredient ingredient;
        public ChangeWithDeleting(Ingredient ingredient)
        {
            IngredientRepository ingredientRepository = new IngredientRepository();
            this.ingredient = ingredient;
            InitializeComponent();
            comboBox1.DataSource = ingredientRepository.GetDataSource().SkipWhile(n => n.Id == ingredient.Id).Select(n => n.Name).ToList();
            comboBox1.SelectedItem = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IngredientRepository ingredientRepository = new IngredientRepository();
            IngredientsForProductRepository ingredientsForProductRepository = new IngredientsForProductRepository();

            Ingredient selectedIngredient = ingredientRepository.GetDataSource().FirstOrDefault(n => n.Name == comboBox1.SelectedItem.ToString());

            if (selectedIngredient == null)
            {
                MessageBox.Show("Інгредієнт не вибрано.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var ingredientInReceipts = ingredientsForProductRepository.GetDataSource().Where(element => element.IngredientId == ingredient.Id).ToList();

            foreach(var item in ingredientInReceipts)
            {
                item.IngredientId = selectedIngredient.Id;

                ingredientsForProductRepository.Edit(item);
            }

            ingredientRepository.Delete(ingredient);

            MessageBox.Show("Інгредієнт успішно замінено і видалено.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
