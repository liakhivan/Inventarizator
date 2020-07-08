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

        BindingSource bsIngredientCollection;
        List<string> ingredientCollection;

        public ChangeWithDeleting(Ingredient ingredient)
        {
            InitializeComponent();
            IngredientRepository ingredientRepository = new IngredientRepository();
            this.ingredient = ingredient;
            ingredientCollection = ingredientRepository.GetDataSource().SkipWhile(n => n.Id == ingredient.Id).Select(n => n.Name).ToList();

            bsIngredientCollection = new BindingSource();
            bsIngredientCollection.DataSource = ingredientCollection;
            comboBox1.DataSource = bsIngredientCollection;
            comboBox1.SelectedItem = null;

            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            comboBox1.AutoCompleteMode = AutoCompleteMode.None;
            comboBox1.IntegralHeight = false;
            comboBox1.SelectedItem = null;
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

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            string searchString = comboBox1.Text;
            Cursor prevCursor = this.Cursor;
            bsIngredientCollection.DataSource = ingredientCollection.Where(x => x.ToUpper().Contains(searchString.ToUpper())).ToList();
            if (bsIngredientCollection.Count != 0)
                comboBox1.DroppedDown = false;
            comboBox1.DroppedDown = true;
            comboBox1.SelectedItem = null;

            comboBox1.Text = searchString;

            // Перенесення курсора в кінець поля вводу.
            comboBox1.Select(searchString.Length, 0);

            this.Cursor = prevCursor;
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
            }
        }
    }
}
