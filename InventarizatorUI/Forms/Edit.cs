﻿using InventarizatorLI.Model;
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

        public Edit()
        {
            InitializeComponent();
            position = button1.Location;
            IngredientRepository source = new IngredientRepository();
            comboBox1.DataSource = source.GetDataSource();
            InitializeComponent();
        }
        
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                ProductRepository productRepository = new ProductRepository();
                comboBox2.DataSource = productRepository.GetDataSource().Select(n => n.Name).ToList();
                this.Height = 295; // +45
                panel2.Enabled = true;
                panel2.Visible = true;
                button1.Location = position;
                IngredientRepository source = new IngredientRepository();
                comboBox1.DataSource = source.GetDataSource();
                maskedTextBox1.Text = textBox1.Text = "";
                receipt.Clear();
                listBox1.DataSource = null;
                listBox1.Items.Clear();
            }
            else
            {
                IngredientRepository productRepository = new IngredientRepository();
                comboBox2.DataSource = productRepository.GetDataSource().Select(n => n.Name).ToList();
                this.Height = 145;
                panel2.Enabled = false;
                panel2.Visible = false;
                button1.Location = panel2.Location;
                textBox1.Text = "";
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
                    else if (receipt.Sum(n => n.Value) != 1)
                        throw new ArgumentException("Сумарна вага інгредієнтів не = 1 кг.");
                    ProductRepository repos = new ProductRepository();
                    Product product = new Product(textBox1.Text);
                    repos.Edit(product, receipt);
                }
                else
                {
                    IngredientRepository repos = new IngredientRepository();
                    repos.Create(new Ingredient(textBox1.Text));
                }

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
                var someElement = receipt.Where(element => element.Key.Name == comboBox1.SelectedItem.ToString()).Select(element => element.Key).FirstOrDefault();
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
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0)
            {
                IngredientRepository source = new IngredientRepository();
                var element = source.GetDataSource().FirstOrDefault(ingredient => listBox1.SelectedItem.ToString().Contains(ingredient.Name));
                var removeElement = receipt.First(n => n.Key.Name == element.Name);
                receipt.Remove(removeElement.Key);
                listBox1.DataSource = receipt.Select(someElement => someElement.Key.ToString() + " " + someElement.Value.ToString()).ToList();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductRepository productRepository = new ProductRepository();
            IngredientsForProductRepository ingredientsForProductRepository = new IngredientsForProductRepository();
            IngredientRepository ingredientRepository = new IngredientRepository();
            List<Ingredient> ingredients = ingredientRepository.GetDataSource();
            textBox1.Text = comboBox2.SelectedItem.ToString();

            Product selectedProduct = productRepository.GetDataSource().First(n => n.Name == textBox1.Text);

            var ingredientsForProduct = ingredientsForProductRepository.GetDataSource().Where(n => n.ProductId == selectedProduct.Id).ToList();

            foreach (var element in ingredientsForProduct)
            {
                Ingredient oneIngredient = ingredients.First(n => n.Id == element.IngredientId);

                receipt.Add(oneIngredient, element.Weight);
            }

            listBox1.DataSource = receipt.Select(n => n.).ToList();
        }
    }
}
