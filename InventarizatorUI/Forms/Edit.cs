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

        private List<string> objectCollection;
        private BindingSource bsObjectCollection;

        private List<string> ingredientForReceiptCollection;
        private BindingSource bsIngredientForReceiptCollection;

        public Edit(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            InitializeComponent();
            position = button1.Location;
            IngredientRepository source = new IngredientRepository();
            ProductRepository productRepository = new ProductRepository();
            ingredientForReceiptCollection = source.GetDataSource().Select(n => n.Name).ToList();

            bsObjectCollection = new BindingSource();

            bsIngredientForReceiptCollection = new BindingSource();
            bsIngredientForReceiptCollection.DataSource = ingredientForReceiptCollection;
            comboBox1.DataSource = bsIngredientForReceiptCollection;
            comboBox1.SelectedItem = null;

            comboBox1.DropDownStyle = comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            comboBox1.AutoCompleteMode = comboBox2.AutoCompleteMode = AutoCompleteMode.None;
            comboBox1.IntegralHeight = comboBox1.IntegralHeight = false;

            radioButton1_CheckedChanged(this, null);
        }

        private void SearchInComboBox(List<string> coll, ref BindingSource bs, ref ComboBox comboBox)
        {
            string searchString = comboBox.Text;
            Cursor prevCursor = this.Cursor;
            bs.DataSource = coll.Where(x => x.ToUpper().Contains(searchString.ToUpper())).ToList();
            if (bs.Count != 0)
                comboBox.DroppedDown = false;
            comboBox.DroppedDown = true;
            //comboBox.SelectedItem = null;

            comboBox.Text = searchString;

            // Перенесення курсора в кінець поля вводу.
            comboBox.Select(searchString.Length, 0);

            this.Cursor = prevCursor;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                ProductRepository productRepository = new ProductRepository();
                objectCollection = productRepository.GetDataSource().Select(n => n.Name).ToList();
                bsObjectCollection.DataSource = objectCollection;
                comboBox2.DataSource = bsObjectCollection;
                comboBox2.SelectedItem = null;

                this.Height = 280;
                panel2.Enabled = true;
                panel2.Visible = true;
                button1.Location = position;
                IngredientRepository source = new IngredientRepository();
                ingredientForReceiptCollection = source.GetDataSource().Select(n => n.Name).ToList();

                bsIngredientForReceiptCollection.DataSource = ingredientForReceiptCollection;
                comboBox1.DataSource = bsIngredientForReceiptCollection;
                comboBox1.SelectedItem = null;

                maskedTextBox1.Text = textBox1.Text = "";
                listBox1.DataSource = null;
                listBox1.Items.Clear();

            }
            else
            {
                IngredientRepository ingredientRepository = new IngredientRepository();

                objectCollection = ingredientRepository.GetDataSource().Select(n => n.Name).ToList();
                bsObjectCollection.DataSource = objectCollection;
                comboBox2.DataSource = bsObjectCollection;
                comboBox2.SelectedItem = null;

                this.Height = 158;
                panel2.Enabled = panel2.Visible = false;
                button1.Location = panel2.Location;
            }
            comboBox2_SelectedIndexChanged(this, null);
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
                    if (1 - weightReceipt > 0.00001 || 1 - weightReceipt < 0)
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
                this.Close();
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
                    listBox1.DataSource = receipt.Select(element => element.Key.ToString() + " " + String.Format("{0:f3}", element.Value)).ToList();
                }
                this.maskedTextBox1.Text = "";
            }
            catch (FormatException)
            {
                MessageBox.Show(@"Не всі поля заповнені.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(@"Інгредієнт не вибраний або заповнені не всі поля для нього.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (comboBox2.SelectedItem == null)
                return;

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

                listBox1.DataSource = receipt.Select(element => element.Key.ToString() + " " + String.Format("{0:f9}", element.Value)).ToList();

            }
            else
            {
                textBox1.Text = comboBox2.SelectedItem.ToString();
            }
        }

        private void comboBox2_TextUpdate(object sender, EventArgs e)
        {
            SearchInComboBox(objectCollection, ref bsObjectCollection, ref comboBox2);
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            SearchInComboBox(ingredientForReceiptCollection, ref bsIngredientForReceiptCollection, ref comboBox1);
        }

        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox1.Focus();
            }
        }

    }
}
