using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories;

namespace InventarizatorUI
{
    public partial class Create : Form
    {
        Point position;
        Dictionary<Ingredient, double> recept = new Dictionary<Ingredient, double>();
        public Create()
        {
            InitializeComponent();
            position = panel3.Location;
            IngredientRepository source = new IngredientRepository();
            comboBox1.DataSource = source.GetDataSource();
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                this.Height = 295;
                panel2.Enabled = true;
                panel2.Visible = true;
                panel3.Location = position;
                IngredientRepository source = new IngredientRepository();
                comboBox1.DataSource = source.GetDataSource();
                maskedTextBox1.Text = textBox1.Text = "";
                recept.Clear();
                listBox1.DataSource = null;
                listBox1.Items.Clear();
            }
            else
            {
                this.Height = 145;
                panel2.Enabled = false;
                panel2.Visible = false;
                panel3.Location = panel2.Location;
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
                    if (recept.Count == 0)
                        throw new ArgumentException("Відсутній рецепт.");
                    else if (recept.Sum(n => n.Value) != 1)
                        throw new ArgumentException("Сумарна вага інгредієнтів не  = 1 кг.");
                    ProductRepository repos = new ProductRepository();
                    Product product = new Product(textBox1.Text);
                    repos.Create(product, recept);
                }
                else
                {
                    IngredientRepository repos = new IngredientRepository();
                    repos.Create(new Ingredient(textBox1.Text));
                }

                MessageBox.Show(@"Об'єкт було успішно створено.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                var someElement = recept.Where(element => element.Key.Name == comboBox1.SelectedItem.ToString()).Select(element => element.Key).FirstOrDefault();

                if (someElement == null)
                {
                    recept.Add(source.GetDataSource().
                        FirstOrDefault(ingredient => ingredient.Name == comboBox1.SelectedItem.ToString()) ?? throw new InvalidOperationException(),
                        Double.Parse(maskedTextBox1.Text));
                    listBox1.DataSource = recept.Select(element => element.Key.ToString() + " " + element.Value.ToString(CultureInfo.InvariantCulture)).ToList();
                }

                this.maskedTextBox1.Text = "";
            } catch(FormatException)
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
                var removeElement = recept.First(n => n.Key.Name == element.Name);
                recept.Remove(removeElement.Key);
                listBox1.DataSource = recept.Select(someElement => someElement.Key.ToString() + " " + someElement.Value.ToString()).ToList();
            }
        }
    }
}
