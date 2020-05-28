using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories;
using InventarizatorLI.Repositories.TableJoin;
using System.Collections.Generic;

namespace InventarizatorUI.Forms
{
    public partial class AddProducts : Form
    {
        Point panel1PositionProductWithoutRemake, panel1PositionProductWithRemake;
        int heightProductsWithoutRemake = 358, heightProductsWithRemake = 500;
        public delegate void Upd();
        private event Upd updateInformation;
        private List<ProductConteiner> productsForRemaking;
        private ProductConteiner elementForRemaking;
        private ProductConteiner defProductForRemaking;

        private List<Conteiner> entryProductsContainerCollection;

        private List<IngredientsForProduct> receipt;



        public AddProducts(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            productsForRemaking = new List<ProductConteiner>();
            InitializeComponent();
            Height = heightProductsWithoutRemake;
            ProductRepository repos = new ProductRepository();

            entryProductsContainerCollection = new List<Conteiner>();

            comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
            groupBox2.Visible = groupBox2.Enabled = false;

            panel1PositionProductWithoutRemake = panel1.Location = new Point(3, 163);
            panel1PositionProductWithRemake = new Point(3, 307);
            dateTimePicker1.MaxDate = DateTime.Today;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!comboBox2.SelectedItem.Equals(null))
                {
                    elementForRemaking = new ProductConteiner(
                        defProductForRemaking.Name,
                        defProductForRemaking.Weight,
                        Decimal.ToInt32(numericUpDown2.Value)
                    );

                    bool wasNotFound = true;
                    listBox1.Items.Clear();

                    for (int index = 0; index < productsForRemaking.Count(); index++)
                    {
                        if (productsForRemaking[index].Name == elementForRemaking.Name & productsForRemaking[index].Weight == elementForRemaking.Weight)
                        {
                            if ((productsForRemaking[index].Amount + elementForRemaking.Amount) > Int32.Parse(label7.Text))
                            {
                                MessageBox.Show(@"Кіликість продукту занадто велика.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                                productsForRemaking[index].Amount += elementForRemaking.Amount;
                            wasNotFound = false;
                        }
                        listBox1.Items.Add(productsForRemaking[index].ToString());
                    }
                    if (wasNotFound)
                    {
                        productsForRemaking.Add(elementForRemaking);
                        listBox1.Items.Add(elementForRemaking.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var productRepository = new ProductRepository();
                defProductForRemaking = productRepository.GetProductConteinerDataSource().
                    First(element => element.ToString() == comboBox2.SelectedItem.ToString());
                label7.Text = defProductForRemaking.Amount.ToString();

            }
            catch (NullReferenceException)
            {
                label7.Text = @"0";
            }
            numericUpDown2.Minimum = 1;
            numericUpDown2.Maximum = Int32.Parse(label7.Text);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                productsForRemaking.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductRepository productRepository = new ProductRepository();
            IngredientRepository ingredientRepository = new IngredientRepository();
            IngredientsForProductRepository receiptRepository = new IngredientsForProductRepository();
            Product currProduct = productRepository.GetDataSource().Where(n => n.Name == comboBox1.SelectedItem.ToString()).FirstOrDefault();

            receipt = receiptRepository.GetDataSource().Where(n => n.ProductId == currProduct.Id).ToList().OrderBy(n => n.Weight).ToList();

            var ingredients = ingredientRepository.GetDataSource();
            comboBox3.DataSource = receipt.Join(
                ingredients,
                elem => elem.IngredientId,
                ingr => ingr.Id,
                (elem, ingr) => ingr.Name).ToList();

            checkBox1.Checked = false;
            comboBox2.Text = "";
            entryProductsContainerCollection.Clear();
            UpdateListBoxProduct();
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ProductRepository productRepository = new ProductRepository();
            IngredientRepository ingredientRepository = new IngredientRepository();
            IngredientsForProductRepository receiptRepository = new IngredientsForProductRepository();

            groupBox1.Enabled = radioButton1.Checked;
            if(!radioButton1.Checked)
            {
                ClearBatch();
            }

            Product currProduct = productRepository.GetDataSource().Where(n => n.Name == comboBox1.SelectedItem.ToString()).FirstOrDefault();

            var receipt = receiptRepository.GetDataSource().Where(n => n.ProductId == currProduct.Id).ToList();

            var ingredientsForProduct = ingredientRepository.GetDataSource();
            comboBox3.DataSource = receipt.Join(
                ingredientsForProduct,
                elem => elem.IngredientId,
                ingr => ingr.Id,
                (elem, ingr) => ingr.Name).ToList();

            checkBox1.Checked = false;
            comboBox2.Text = "";
            entryProductsContainerCollection.Clear();
            UpdateListBoxProduct();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                entryProductsContainerCollection.RemoveAt(listBox2.SelectedIndex);
                UpdateListBoxProduct();
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        private void ClearBatch()
        {
            maskedTextBox2.Text = "";
            maskedTextBox3.Text = "";
        }

        private void UpdateListBoxProduct()
        {
            int lemght = comboBox1.SelectedItem.ToString().Split('\"').Length;
            listBox2.DataSource = entryProductsContainerCollection.Select(n => $"\"{comboBox1.SelectedItem.ToString().Split('\"')[lemght - 2]}\" {n.Amount} шт.  по  {String.Format("{0:f2}", n.Weight)} кг.").ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                ProductRepository repos = new ProductRepository();
                double weight = double.Parse(maskedTextBox1.Text);

                int amount = Decimal.ToInt32(numericUpDown1.Value);

                int productId = repos.GetDataSource().Where(n => n.Name == comboBox1.SelectedItem.ToString()).FirstOrDefault().Id;

                if (weight < 0)
                {
                    MessageBox.Show(@"Некоректна вага продукту.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (amount < 0)
                {
                    MessageBox.Show(@"Некоректна кількість продукту.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Conteiner entryProductConteiner = new Conteiner(productId, weight, amount);

                var isOwn = entryProductsContainerCollection.Where(n => n.ProductId == entryProductConteiner.ProductId && n.Weight == entryProductConteiner.Weight).FirstOrDefault() != null;

                if(isOwn)
                {
                    entryProductsContainerCollection.Where(n => n.ProductId == entryProductConteiner.ProductId).First().Amount++;
                }
                else
                {
                    entryProductsContainerCollection.Add(entryProductConteiner);
                }

                UpdateListBoxProduct();
            }
            catch(FormatException)
            {
                MessageBox.Show(@"Введено некоректні дані продукту.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Visible = groupBox2.Enabled = checkBox1.Checked;
            maskedTextBox1.Text = "";
            numericUpDown1.Value = 1;


            elementForRemaking = null;
            if (checkBox1.Checked)
            {
                Height = heightProductsWithRemake;
                panel1.Location = panel1PositionProductWithRemake;
                comboBox2.Enabled = checkBox1.Checked;
                numericUpDown2.Value = 1;
                listBox1.Items.Clear();

                ProductRepository repos = new ProductRepository();

                try
                {
                    // Оскільки деякі продукти, 
                    // які мають спільний головний інгредієнт 
                    // можна переробити, то ми витягуємо список 
                    // всіх продуктів які мають спільний головний інгредієнт.

                    ////////var name = comboBox1.SelectedItem.ToString().Substring(comboBox1.SelectedItem.ToString().IndexOf("\""));
                    ////////var data = repos.GetProductConteinerDataSource().
                    ////////Where(elem => elem.Name.Contains(name) & elem.Amount != 0 & elem.Weight <= 6).
                    ////////Select(element => $"{element.Name} {element.Weight}").ToList();

                    List<string> data = repos.GetProductConteinerDataSource().Where(elem => elem.Amount != 0 & elem.Weight <= 6).Select(n => n.ToString()).ToList();
                    if (data.Count == 0)
                    {
                        checkBox1.Checked = false;
                        MessageBox.Show(@"Продукту для переробки не існує", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        comboBox2.DataSource = data;
                }
                catch (Exception)
                {
                    comboBox2.DataSource = null;
                }
            }
            else
            {
                this.Height = heightProductsWithoutRemake;
                panel1.Location = panel1PositionProductWithoutRemake;
            }
            groupBox2.Visible = groupBox2.Enabled = checkBox1.Checked;
            maskedTextBox1.Text = "";
            numericUpDown1.Value = 1;
        }

        private void RemoveProductsForRemaking()
        {

            ProductRepository productRepository = new ProductRepository();

            ConteinerRepository conteinerRepository = new ConteinerRepository();

            foreach (var elementOfRemaking in productsForRemaking)
            {

                var product = productRepository.GetDataSource()
                    .First(element => element.Name == elementOfRemaking.Name);

                var conteiner = conteinerRepository.GetDataSource()
                    .First(elem => elem.ProductId == product.Id & elem.Weight == elementOfRemaking.Weight);

                conteinerRepository.Remove(conteiner.Id, dateTimePicker1.Value, 3, elementOfRemaking.Amount);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(radioButton1.Checked)
                {

                    ConteinerRepository conteinerRepository = new ConteinerRepository();
                    IngredientsForProductRepository receiptRepository = new IngredientsForProductRepository();
                    IngredientRepository ingredientRepository = new IngredientRepository();

                    double weight = Double.Parse(maskedTextBox2.Text);
                    double amount = Double.Parse(maskedTextBox3.Text);
                    double weightIngredientKeyBatch = weight * amount;
                    double weightAllProducts = entryProductsContainerCollection.Sum(n => n.Amount * n.Weight);
                    double weightForRemaking = 0;


                    var ingredients = ingredientRepository.GetDataSource();
                    var detailReceipt = receipt.Join(
                        ingredients,
                        elem => elem.IngredientId,
                        ingr => ingr.Id,
                        (elem, ingr) => new {
                            ingr.Name,
                            elem.Weight
                        }).ToList();
                    
                    double weightIngredientKeyOnOneKilo = detailReceipt.First(n => n.Name == comboBox3.SelectedItem.ToString()).Weight;

                    double weightBatch = weightIngredientKeyBatch / weightIngredientKeyOnOneKilo;


                    if (weight <= 0)
                    {
                        MessageBox.Show(@"Некоректна вага інгредієнту - ключа на 1 заміс", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                
                    if (amount <= 0)
                    {
                        MessageBox.Show(@"Некоректна кількість замісів", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (checkBox1.Checked)
                    {
                        weightForRemaking = productsForRemaking.Sum(n => n.Amount * n.Weight);
                    }

                    if (weightAllProducts > weightBatch + weightForRemaking || weightAllProducts == 0)
                    {

                        MessageBox.Show($"Некоректна вага всіх продуктів.\nВидаліть зайві продукти, збільшіть кількість замісів,\nабо додайте переробку.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    RemoveProductsForRemaking();
                    conteinerRepository.AddCollectionContainers(entryProductsContainerCollection, dateTimePicker1.Value, weightBatch);
                }
                else
                {
                    ConteinerRepository conteinerRepository = new ConteinerRepository();

                    double weightAllProducts = entryProductsContainerCollection.Sum(n => n.Amount * n.Weight);
                    double weightForRemaking = 0;

                    if (checkBox1.Checked)
                    {
                        weightForRemaking = productsForRemaking.Sum(n => n.Amount * n.Weight);
                    }

                    if (weightAllProducts > weightForRemaking || weightAllProducts == 0)
                    {
                        MessageBox.Show($"Некоректна вага всіх продуктів.\nВидаліть зайві продукти, збільшіть кількість замісів,\nабо додайте переробку.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    RemoveProductsForRemaking();
                    conteinerRepository.AddCollectionContainers(entryProductsContainerCollection, dateTimePicker1.Value, 0);
                }
                checkBox1.Checked = false;

                UpdateListBoxProduct();
                updateInformation();
                
                        MessageBox.Show($"Операція успішно виконана.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
