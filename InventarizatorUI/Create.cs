using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if(radioButton1.Checked == true)
            {
                this.Height = 295;
                panel2.Enabled = true;
                panel2.Visible = true;
                panel3.Location = position;
                IngredientRepository source = new IngredientRepository();
                comboBox1.DataSource = source.GetDataSource();
                label2.Text = "Повідомлення про збереження.";
                textBox1.Text = "";
            }
            else
            {
                this.Height = 145;
                panel2.Enabled = false;
                panel2.Visible = false;
                panel3.Location = panel2.Location;
                label2.Text = "Повідомлення про збереження.";
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
                    if ( recept.Count == 0)
                        throw new ArgumentException();
                    ProductRepository repos = new ProductRepository();
                    Product product = new Product(textBox1.Text);
                    repos.Create(new Product(textBox1.Text), recept);
                    CreateRecept form3 = new CreateRecept(product);
                }
                else
                {
                    IngredientRepository repos = new IngredientRepository();
                    repos.Create(new Ingredient(textBox1.Text));
                }
                label2.Text = "Об'єкт було успішно створено.";
            }
            catch (ArgumentNullException)
            {
                label2.Text = "Не всі поля заповнені.";
            }
            catch (ArgumentException)
            {
                label2.Text = "Цей об'єкт неможливо створити.";
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
                    recept.Add(source.GetDataSource()
                        .Where(ingredient => ingredient.Name == comboBox1.SelectedItem.ToString()).FirstOrDefault(),
                        Double.Parse(maskedTextBox1.Text));
                    listBox1.DataSource = recept.Select(element => element.Key.ToString() + " " + element.Value.ToString()).ToList();
                }
            } catch(FormatException)
            {
                label2.Text = "Не всі поля заповнені.";
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            IngredientRepository source = new IngredientRepository();
            var element = source.GetDataSource().Where(ingredient => listBox1.SelectedItem.ToString().Contains(ingredient.Name)).FirstOrDefault();
            var removeElement = recept.First(n => n.Key.Name == element.Name);
            var checker = recept.Remove(removeElement.Key);
            listBox1.DataSource = recept.Select(someElement => someElement.Key.ToString() + " " + someElement.Value.ToString()).ToList();
        }
    }
}
