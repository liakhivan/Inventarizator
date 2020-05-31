using InventarizatorLI.Model;
using InventarizatorLI.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventarizatorUI.Forms
{
    public partial class Edit : Form
    {
        Point position;
        Dictionary<Ingredient, double> receipt = new Dictionary<Ingredient, double>();
        Product currProduct;
        public delegate void Upd();
        private event Upd updateInformation;

        public Edit(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            InitializeComponent();
            position = button1.Location;
            IngredientRepository source = new IngredientRepository();
            ProductRepository productRepository = new ProductRepository();
            comboBox1.DataSource = source.GetDataSource().Select(n => n.Name).ToList();
            comboBox1.DataSource = source.GetDataSource();
            comboBox1.SelectedIndex = -1;
            comboBox2.DataSource = productRepository.GetDataSource().Select(n => n.Name).ToList();
        }
        
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                ProductRepository productRepository = new ProductRepository();
                comboBox2.DataSource = productRepository.GetDataSource().Select(n => n.Name).ToList();
                this.Height = 280;
                panel2.Enabled = true;
                panel2.Visible = true;
                button1.Location = position;
                IngredientRepository source = new IngredientRepository();
                comboBox1.DataSource = source.GetDataSource();
                comboBox1.SelectedIndex = -1;
                maskedTextBox1.Text = textBox1.Text = "";
                listBox1.DataSource = null;
                listBox1.Items.Clear();

            }
            else
            {
                IngredientRepository ingredientRepository = new IngredientRepository();
                comboBox2.DataSource = ingredientRepository.GetDataSource().Select(n => n.Name).ToList();
                this.Height = 158;
                panel2.Enabled = panel2.Visible = false;
                button1.Location = panel2.Location;
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "")
                    throw new ArgumentNullException();

                if (radioButton1.Checked)
                {
                    if (receipt.Count == 0)
                        throw new ArgumentException("Відсутній рецепт.");
                    double weightReceipt = receipt.Sum(n => n.Value);
                    if (1 - weightReceipt > 0.00001)
                        throw new ArgumentException("Сумарна вага інгредієнтів не = 1 кг.");

                    ProductRepository productRepository = new ProductRepository();
                    Product product = productRepository.GetDataSource().FirstOrDefault(n => n.Name == comboBox2.SelectedItem.ToString());
                    product.Name = textBox1.Text;
                    productRepository.Edit(product, receipt);
                }
                else
                {
                    IngredientRepository ingredientRepository = new IngredientRepository();
                    Ingredient currIngredient = ingredientRepository.GetDataSource().First(n => n.Name == comboBox2.SelectedItem.ToString());

                    currIngredient.Name = textBox1.Text;
                    ingredientRepository.Edit(currIngredient);
                }
                updateInformation();
                MessageBox.Show(@"Об'єкт було успішно відредаговано.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show(@"Не всі поля заповнені.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                IngredientRepository source = new IngredientRepository();

                var someElement = receipt.FirstOrDefault(n => n.Key.Name == comboBox1.SelectedItem.ToString()).Key;

                if (someElement == null)
                {
                    receipt.Add(source.GetDataSource().
                        FirstOrDefault(ingredient => ingredient.Name == comboBox1.SelectedItem.ToString()) ?? throw new InvalidOperationException(),
                        Double.Parse(maskedTextBox1.Text));
                    listBox1.DataSource = receipt.Select(element => element.Key.ToString() + " " + element.Value.ToString(CultureInfo.InvariantCulture)).ToList();
                }
                this.maskedTextBox1.Text = "";
            }
            catch (FormatException)
            {
                MessageBox.Show(@"Не всі поля заповнені.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(@"Інгредієнт не вибраний або заповнені не всі .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                return;
            }

            IngredientRepository ingredientRepository = new IngredientRepository();

            Ingredient ingredientForRemove = ingredientRepository.GetDataSource().FirstOrDefault(ingredient => listBox1.SelectedItem.ToString().Contains(ingredient.Name));

            receipt.Remove(receipt.First(n => n.Key.Id == ingredientForRemove.Id).Key);

            listBox1.DataSource = receipt.Select(someElement => someElement.Key.ToString() + " " + someElement.Value.ToString()).ToList();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                ProductRepository productRepository = new ProductRepository();
                IngredientsForProductRepository ingredientsForProductRepository = new IngredientsForProductRepository();
                IngredientRepository ingredientRepository = new IngredientRepository();
                List<Ingredient> ingredients = ingredientRepository.GetDataSource();
                textBox1.Text = comboBox2.SelectedItem.ToString();

                receipt.Clear();

                currProduct = productRepository.GetDataSource().First(n => n.Name == textBox1.Text);

                var currReceipt = ingredientsForProductRepository.GetDataSource().Where(n => n.ProductId == currProduct.Id);

                foreach (var oneElementInReceipt in currReceipt)
                {
                    Ingredient ingredient = ingredients.First(n => n.Id == oneElementInReceipt.IngredientId);
                    receipt.Add(ingredient, oneElementInReceipt.Weight);
                }

                var ingredientsForProduct = ingredientsForProductRepository.GetDataSource().Where(n => n.ProductId == currProduct.Id).ToList();

                listBox1.DataSource = receipt.Select(someElement => someElement.Key.ToString() + " " + someElement.Value.ToString()).ToList();

            }
            else
            {
                textBox1.Text = comboBox2.SelectedItem.ToString();
            }
        }
    }
}
