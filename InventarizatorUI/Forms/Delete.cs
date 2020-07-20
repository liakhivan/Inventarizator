using InventarizatorLI.Model;
using InventarizatorLI.Repositories;
using InventarizatorUI.Forms;
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
            if (radioButton1.Checked)
            {
                ProductRepository repos = new ProductRepository();
                listBox1.DataSource = repos.GetDataSource();
            }
            else if (radioButton2.Checked)
            {
                IngredientRepository repos = new IngredientRepository();
                listBox1.DataSource = repos.GetDataSource();
            }
            else
            {
                TareRepository tareRepository = new TareRepository();
                listBox1.DataSource = tareRepository.GetDataSource().Select(n => n.Name ).ToList<string>();
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
                    updateInformation();
                }
                else if (radioButton2.Checked)
                {
                    IngredientRepository ingredientRepository = new IngredientRepository();
                    IngredientsForProductRepository ingredientsForProductRepository = new IngredientsForProductRepository();

                    Ingredient ingredient = ingredientRepository.GetDataSource().First(element => element.Name == listBox1.SelectedItem.ToString());

                    var receiptsWithCurrIngredient = ingredientsForProductRepository.GetDataSource().Where(n => n.IngredientId == ingredient.Id).ToList();

                    if (receiptsWithCurrIngredient.Count != 0)
                    {
                        DeleteMessage deleteMessage = new DeleteMessage(ingredient, updateInformation);
                        deleteMessage.ShowDialog();
                    }
                    else
                    {
                        ingredientRepository.Delete(ingredient);

                        updateInformation();
                        MessageBox.Show(@"Видалення успішне.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    RadioButton1_CheckedChanged(this, null);
                } 
                else
                {
                    TareRepository tareRepository = new TareRepository();

                    Tare tareForDeleting = tareRepository.GetDataSource().First(n => n.Name == listBox1.SelectedItem.ToString());

                    tareRepository.DeleteById(tareForDeleting.Id);

                    updateInformation();
                    MessageBox.Show(@"Видалення успішне.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    RadioButton1_CheckedChanged(this, null);
                }
            }
            catch (InvalidOperationException exeption)
            {
                MessageBox.Show(exeption.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton1_CheckedChanged(this, null);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton1_CheckedChanged(this, null);
        }
    }
}
