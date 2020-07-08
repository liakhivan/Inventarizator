using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InventarizatorLI.Repositories;

namespace InventarizatorUI
{
    public partial class Remove : Form
    {
        public delegate void Upd();
        private event Upd updateInformation;

        BindingSource bsObjectForRemowingCollection;
        List<string> objectForRemowingCollection;

        public Remove(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            var productRepository = new ProductRepository();
            InitializeComponent();
            RadioButton1_CheckedChanged(this, null);
            maskedTextBox1.Visible = maskedTextBox1.Enabled = false;
            numericUpDown1.Visible = numericUpDown1.Enabled = true;
            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker1.Value = DateTime.Today;
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
                    conteinerRepository.Remove(conteiner.Id, dateTimePicker1.Value, 1, Decimal.ToInt32(numericUpDown1.Value));
                }
                else
                {
                    var ingredientRepository = new IngredientRepository();
                    var packageRepository = new PackageRepository();
                    var ingredientPackage = ingredientRepository.GetIngredientPackageDataSource().
                           First(element => $"{element.Name} {element.Weight}" == comboBox1.SelectedItem.ToString());
                    var ingredient = ingredientRepository.GetDataSource().First(element => element.Name == ingredientPackage.Name);
                    var conteiner = packageRepository.GetDataSource().First(elem => elem.IngredientId == ingredient.Id & elem.Weight == ingredientPackage.Weight);
                    packageRepository.Remove(conteiner.Id, dateTimePicker1.Value, Double.Parse(maskedTextBox1.Text));
                }
                RadioButton1_CheckedChanged(null, null);

                MessageBox.Show(@"Об'єкт успішно списано.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updateInformation();

            }  catch(Exception)
            {
                MessageBox.Show(@"Помилка списання", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                var productRepository = new ProductRepository();

                bsObjectForRemowingCollection = new BindingSource();
                objectForRemowingCollection = productRepository.GetProductConteinerDataSource().
                Select(elem => $"{elem.Name} {elem.Weight}").ToList();
                bsObjectForRemowingCollection.DataSource = objectForRemowingCollection;
                comboBox1.DataSource = bsObjectForRemowingCollection;
                comboBox1.SelectedItem = null;

                comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
                comboBox1.AutoCompleteMode = AutoCompleteMode.None;
                comboBox1.IntegralHeight = false;

                maskedTextBox1.Visible = maskedTextBox1.Enabled = false;
                numericUpDown1.Visible = numericUpDown1.Enabled = true;
                numericUpDown1.Value = 1;
            }
            else
            {
                var ingredientRepository = new IngredientRepository();

                bsObjectForRemowingCollection = new BindingSource();
                objectForRemowingCollection = ingredientRepository.GetIngredientPackageDataSource().
                Select(elem => $"{elem.Name} {elem.Weight}").ToList();
                bsObjectForRemowingCollection.DataSource = objectForRemowingCollection;
                comboBox1.DataSource = bsObjectForRemowingCollection;
                comboBox1.SelectedItem = null;

                comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
                comboBox1.AutoCompleteMode = AutoCompleteMode.None;
                comboBox1.IntegralHeight = false;

                maskedTextBox1.Visible = maskedTextBox1.Enabled = true;
                numericUpDown1.Visible = numericUpDown1.Enabled = false;
                maskedTextBox1.Text = "";
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (comboBox1.SelectedItem == null)
                    return;
                var productRepository = new ProductRepository();
                var conteinerRepository = new ConteinerRepository();
                var productConteiner = productRepository.GetProductConteinerDataSource().
                       First(element => $"{element.Name} {element.Weight}" == comboBox1.SelectedItem.ToString());
                var product = productRepository.GetDataSource().First(element => element.Name == productConteiner.Name);
                var conteiner = conteinerRepository.GetDataSource().First(elem => elem.ProductId == product.Id & elem.Weight == productConteiner.Weight);
                numericUpDown1.Maximum = conteiner.Amount;
            }
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            string searchString = comboBox1.Text;
            Cursor prevCursor = this.Cursor;
            bsObjectForRemowingCollection.DataSource = objectForRemowingCollection.Where(x => x.ToUpper().Contains(searchString.ToUpper())).ToList();
            if (bsObjectForRemowingCollection.Count != 0)
                comboBox1.DroppedDown = false;
            comboBox1.DroppedDown = true;
            comboBox1.SelectedItem = null;

            comboBox1.Text = searchString;

            // Перенесення курсора в кінець поля вводу.
            comboBox1.Select(searchString.Length, 0);

            this.Cursor = prevCursor;
        }
    }
}
