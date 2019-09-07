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
                label1.Text = $"Назва продукту:";
                panel1.Visible = panel1.Enabled = true;
                panel2.Visible = panel2.Enabled = true;
            }
            else
            {
                IngredientRepository repos = new IngredientRepository();
                comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
                label1.Text = $"Назва інгредієнта";
                panel1.Visible = panel1.Enabled = false;
                panel2.Visible = panel2.Enabled = false;
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                comboBox2.Enabled = true;
            }
            else
            {
                comboBox2.Enabled = false;
            }

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
                    label1.Text = $"Назва продукту:";
                    double weight;
                    Double.TryParse(maskedTextBox1.Text, out weight);
                    ConteinerRepository repository = new ConteinerRepository();
                    repository.Create(new Conteiner(comboBox1.SelectedIndex + 1, weight, Decimal.ToInt32(numericUpDown1.Value)));
                }
                else
                {
                    label1.Text = $"Назва інгредієнту:";
                    double weight;
                    Double.TryParse(maskedTextBox1.Text, out weight);
                    PackageRepository repository = new PackageRepository();
                    repository.Create(new Package(comboBox1.SelectedIndex + 1, weight));
                }
            }
            catch (FormatException)
            {
                label5.Text = $"Введено некоректні дані.";
            }
        }
    }
}
