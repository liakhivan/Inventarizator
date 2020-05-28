using System;
using System.Linq;
using System.Drawing;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories;
using InventarizatorLI.Repositories.TableJoin;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InventarizatorUI.Forms
{
    public partial class AddIngradients : Form
    {
        public delegate void Upd();
        private event Upd updateInformation;

        public AddIngradients(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            InitializeComponent();

            IngredientRepository repos = new IngredientRepository();
            comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
            comboBox1.SelectedIndex = -1;
            label1.Text = @"Назва:";
            maskedTextBox1.Mask = @"000.00";
            maskedTextBox1.Text = "";

            dateTimePicker1.MaxDate = DateTime.Today;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                Double.TryParse(maskedTextBox1.Text, out var weight);
                PackageRepository repository = new PackageRepository();
                IngredientRepository ingredientRepository = new IngredientRepository();

                string currIngredientName = comboBox1.SelectedItem.ToString();

                int id = ingredientRepository.GetDataSource().First(element => element.Name == currIngredientName).Id;
                repository.Add(new Package(id, weight), dateTimePicker1.Value);

                updateInformation();
                MessageBox.Show(@"Інгредієнт було успішно додано.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show(@"Некоректна вага інгредієнту.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show($"Інгредієнт не вибрано.\nБудь ласка виберіть потрібний інгредієнт", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ;
            }
            catch (Exception exception)
            {
                //TODO: Program thrown message on english when user try to add some product. Also check this problem with ingredient.
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                maskedTextBox1.Focus();
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
                button1.Focus();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            maskedTextBox1.Text = "";
            dateTimePicker1.Value = DateTime.Today;
        }
    }
}