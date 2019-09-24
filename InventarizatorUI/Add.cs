using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories;
using InventarizatorLI.Repositories.TableJoin;

namespace InventarizatorUI
{
    public partial class Add : Form
    {
        Point panel1Position1, panel1Position2, infoPosition1, infoPosition2;
        public delegate void Upd();
        private event Upd updateInformation;
        public Add(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            InitializeComponent();
            ProductRepository repos = new ProductRepository();
            comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
            checkBox1.Checked = false;
            comboBox2.Enabled = false;
            panel1Position1 = panel1.Location;
            panel1Position2 = panel2.Location;
            infoPosition1 = label5.Location;
            infoPosition2 = label5.Location;
            infoPosition2.Y -= 20;
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                ProductRepository repos = new ProductRepository();
                comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
                label1.Text = @"Назва продукту:";
                panel3.Visible = panel3.Enabled = true;
                panel2.Visible = panel2.Enabled = true;
                panel1.Location = panel1Position1;
                checkBox2.Enabled = checkBox2.Visible = true;
                label5.Location = infoPosition1;
                Height += 54;
                maskedTextBox1.Mask = @"0.00";
            }
            else
            {
                IngredientRepository repos = new IngredientRepository();
                comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
                label1.Text = @"Назва інгредієнта";
                panel3.Visible = panel3.Enabled = false;
                panel2.Visible = panel2.Enabled = false;
                panel1.Location = panel1Position2;
                checkBox2.Enabled = checkBox2.Visible = false;
                label5.Location = infoPosition2;
                Height -= 54;
                maskedTextBox1.Mask = @"000.00";
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            ProductRepository repos = new ProductRepository();
            comboBox2.Enabled = checkBox1.Checked;
            comboBox2.DataSource = repos.GetProductConteinerDataSource().
                Where(elem => elem.Name == comboBox1.SelectedItem.ToString() & elem.Amount != 0 & elem.Weight <= 3).
                Select(element => $"{element.Name} {element.Weight}").ToList();
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
                    double recreate = 0;
                    if (checkBox1.Checked)
                    {
                        var productConteiner = productRepository.GetProductConteinerDataSource().
                        First(element => $"{element.Name} {element.Weight}" == comboBox2.SelectedItem.ToString());
                        recreate = productConteiner.Weight;
                        var product = productRepository.GetDataSource().First(element => element.Name == productConteiner.Name);
                        var conteiner = conteinerRepository.GetDataSource().First(elem => elem.ProductId == product.Id & elem.Weight == recreate);
                        conteinerRepository.Remove(conteiner.Id);
                    }

                    int id = productRepository.GetDataSource()
                        .First(element => element.Name == comboBox1.SelectedItem.ToString()).Id;
                    conteinerRepository.Create(
                        new Conteiner(id, weight, Decimal.ToInt32(numericUpDown1.Value)),
                        recreate,
                        checkBox2.Checked
                        );
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
                updateInformation();
            }
            catch (Exception)
            {
                label5.ForeColor = System.Drawing.Color.Red;
                label5.Text = @"Введено некоректні дані.";
            }
        }

        private void Add_MouseMove(object sender, MouseEventArgs e)
        {
            label5.ForeColor = System.Drawing.Color.Black;
            label5.Text = @"Інформація про додавання.";
        }
    }
}
