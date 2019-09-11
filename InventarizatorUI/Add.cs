using System;
using System.Linq;
using System.Windows.Forms;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories;

namespace InventarizatorUI
{
    public partial class Add : Form
    {
        public Add()
        {
            InitializeComponent();
            ProductRepository repos = new ProductRepository();
            comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
            checkBox1.Checked = false;
            comboBox2.Enabled = false;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ComboBox1_MouseClick(object sender, MouseEventArgs e)
        {

            
        }


        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                ProductRepository repos = new ProductRepository();
                comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
                label1.Text = @"Назва продукту:";
                panel1.Visible = panel1.Enabled = true;
                panel2.Visible = panel2.Enabled = true;
                this.maskedTextBox1.Mask = @"0.00";
            }
            else
            {
                IngredientRepository repos = new IngredientRepository();
                comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
                label1.Text = @"Назва інгредієнта";
                panel1.Visible = panel1.Enabled = false;
                panel2.Visible = panel2.Enabled = false;
                this.maskedTextBox1.Mask = @"000.00";
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = checkBox1.Checked;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    if (numericUpDown1.Value <= 0)
                    {
                        throw new FormatException();
                    }
                    double weight;
                    Double.TryParse(maskedTextBox1.Text, out weight);
                    ConteinerRepository conteinerRepository = new ConteinerRepository();
                    ProductRepository productRepository = new ProductRepository();
                    int id = productRepository.GetDataSource()
                        .First(element => element.Name == comboBox1.SelectedItem.ToString()).Id;
                    conteinerRepository.Create(new Conteiner(id, weight, Decimal.ToInt32(numericUpDown1.Value)));
                }
                else
                {
                    Double.TryParse(maskedTextBox1.Text, out var weight);
                    PackageRepository repository = new PackageRepository();
                    IngredientRepository ingredientRepository = new IngredientRepository();
                    int id = ingredientRepository.GetDataSource()
                        .First(element => element.Name == comboBox1.SelectedItem.ToString()).Id;
                    repository.Create(new Package(id, weight));
                }

                label5.ForeColor = System.Drawing.Color.Green;
                label5.Text = @"Об'єкт було успішно додано.";
            }
            catch (FormatException)
            {
                label5.ForeColor = System.Drawing.Color.Red;
                label5.Text = @"Введено некоректні дані.";
            }
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Add_MouseMove(object sender, MouseEventArgs e)
        {
            label5.ForeColor = System.Drawing.Color.Black;
            label5.Text = @"Інформація про додавання.";
        }
    }
}
