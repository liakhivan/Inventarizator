using System;
using System.Linq;
using System.Windows.Forms;
using InventarizatorLI.Repositories;

namespace InventarizatorUI
{
    public partial class Remove : Form
    {
        public delegate void Upd();
        private event Upd updateInformation;
        public Remove(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            var productRepository = new ProductRepository();
            InitializeComponent();
            comboBox1.DataSource = productRepository.GetProductConteinerDataSource().
                Select(elem => $"{elem.Name} {elem.Weight}").ToList();
            maskedTextBox1.Visible = maskedTextBox1.Enabled = false;
            numericUpDown1.Visible = numericUpDown1.Enabled = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    var productRepository = new ProductRepository();
                    var conteinerRepository = new ConteinerRepository();
                    var productConteiner = productRepository.GetProductConteinerDataSource().
                           First(element => $"{element.Name} {element.Weight}" == comboBox1.SelectedItem.ToString());
                    var product = productRepository.GetDataSource().First(element => element.Name == productConteiner.Name);
                    var conteiner = conteinerRepository.GetDataSource().First(elem => elem.ProductId == product.Id & elem.Weight == productConteiner.Weight);
                    conteinerRepository.Remove(conteiner.Id, Decimal.ToInt32(numericUpDown1.Value));
                }
                else
                {
                    var ingredientRepository = new IngredientRepository();
                    var packageRepository = new PackageRepository();
                    var ingredientPackage = ingredientRepository.GetIngredientPackageDataSource().
                           First(element => $"{element.Name} {element.Weight}" == comboBox1.SelectedItem.ToString());
                    var ingredient = ingredientRepository.GetDataSource().First(element => element.Name == ingredientPackage.Name);
                    var conteiner = packageRepository.GetDataSource().First(elem => elem.IngredientId == ingredient.Id & elem.Weight == ingredientPackage.Weight);
                    packageRepository.Remove(conteiner.Id, Double.Parse(maskedTextBox1.Text));
                }
                RadioButton1_CheckedChanged(null, null);
                label3.ForeColor = System.Drawing.Color.Green;
                label3.Text = @"Об'єкт успішно списано.";
                updateInformation();

            }  catch(Exception)
            {
                label3.ForeColor = System.Drawing.Color.Red;
                label3.Text = @"Помилка списання";
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                var productRepository = new ProductRepository();
                comboBox1.DataSource = productRepository.GetProductConteinerDataSource().
                Select(elem => $"{elem.Name} {elem.Weight}").ToList();
                maskedTextBox1.Visible = maskedTextBox1.Enabled = false;
                numericUpDown1.Visible = numericUpDown1.Enabled = true;
            }
            else
            {
                var ingredientRepository = new IngredientRepository();
                comboBox1.DataSource = ingredientRepository.GetIngredientPackageDataSource().
                Select(elem => $"{elem.Name} {elem.Weight}").ToList();
                maskedTextBox1.Visible = maskedTextBox1.Enabled = true;
                numericUpDown1.Visible = numericUpDown1.Enabled = false;
            }
        }

        private void Label3_MouseMove(object sender, MouseEventArgs e)
        {
            label3.ForeColor = System.Drawing.Color.Black;
            label3.Text = @"Інформація про списання.";
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                var productRepository = new ProductRepository();
                var conteinerRepository = new ConteinerRepository();
                var productConteiner = productRepository.GetProductConteinerDataSource().
                       First(element => $"{element.Name} {element.Weight}" == comboBox1.SelectedItem.ToString());
                var product = productRepository.GetDataSource().First(element => element.Name == productConteiner.Name);
                var conteiner = conteinerRepository.GetDataSource().First(elem => elem.ProductId == product.Id & elem.Weight == productConteiner.Weight);
                numericUpDown1.Maximum = conteiner.Amount;
            }
        }
    }
}
