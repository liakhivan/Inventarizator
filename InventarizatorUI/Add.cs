using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using InventarizatorLI.Model;
using System.Collections.Generic;
using InventarizatorLI.Repositories;

namespace InventarizatorUI
{
    public partial class Add : Form
    {
        Point panel1Position1, panel1Position2, label5Position1, label5Position2, checkBox2Position1, checkBox2Position2;
        public delegate void Upd();
        private event Upd updateInformation;


        public Add(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            InitializeComponent();
            Height -= 90;
            ProductRepository repos = new ProductRepository();
            comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
            checkBox1.Checked = false;
            comboBox2.Enabled = false;
            panel1Position1 = panel1.Location = new Point(9, 106);
            panel1Position2 = panel2.Location = new Point(9, 60);
            label5Position1 = label5.Location = new Point(127, 84);
            label5Position2 = label2.Location;
            label5Position2.Y += 27;
            checkBox2Position1 = checkBox2.Location = new Point(17, 84);
            checkBox2Position2 = new Point(17, 178);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (!comboBox2.SelectedItem.Equals(null))
            {
                var newElementOfListBox = (numericUpDown2.Value + @" " + comboBox2.SelectedItem.ToString());
                if (!listBox1.Items.Contains(newElementOfListBox))
                    listBox1.Items.Add(newElementOfListBox);
            }
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var productRepository = new ProductRepository();
                var currentProductConteinerForRemaking = productRepository.GetProductConteinerDataSource().
                    First(element => $"{element.Name} {element.Weight}" == comboBox2.SelectedItem.ToString());
                label7.Text = currentProductConteinerForRemaking.Amount.ToString();
            }
            catch(NullReferenceException)
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
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            catch(ArgumentOutOfRangeException)
            { }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                ProductRepository repos = new ProductRepository();
                try
                {
                    // Оскільки деякі продукти, 
                    // які мають спільний головний інгредієнт 
                    // можна переробити, то ми витягуємо список 
                    // всіх продуктів які мають спільний головний інгредієнт.
                    var name = comboBox1.SelectedItem.ToString().Substring(comboBox1.SelectedItem.ToString().IndexOf("\""));
                    var data = repos.GetProductConteinerDataSource().
                    Where(elem => elem.Name.Contains(name) & elem.Amount != 0 & elem.Weight <= 3).
                    Select(element => $"{element.Name} {element.Weight}").ToList();

                    comboBox2.DataSource = (data.Count == 0) ? null : data;
                }
                catch (Exception)
                {
                    comboBox2.DataSource = null;
                }
                listBox1.Items.Clear();
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                ProductRepository repos = new ProductRepository();
                comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
                label1.Text = @"Назва:";
                panel1.Location = panel1Position1;
                label5.Location = label5Position1;
                Height += 45;
                panel2.Visible = panel2.Enabled = true;
                panel3.Visible = panel3.Enabled = true;
                checkBox2.Enabled = checkBox2.Visible = true;
                maskedTextBox1.Mask = @"0.00";
            }
            else
            {
                IngredientRepository repos = new IngredientRepository();
                comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
                label1.Text = @"Назва:";
                panel2.Visible = panel2.Enabled = false;
                panel3.Visible = panel3.Enabled = false;
                panel4.Visible = panel4.Enabled = false;
                checkBox2.Checked = checkBox1.Checked = false;
                checkBox2.Enabled = checkBox2.Visible = false;
                panel1.Location = panel1Position2;
                label5.Location = label5Position2;
                Height -= 45;
                maskedTextBox1.Mask = @"000.00";
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                this.Height += 90;
                panel1.Location = new Point(4, 200);
                label5.Location = new Point(127, 179);
                checkBox2.Location = checkBox2Position2;
                panel4.Visible = panel4.Enabled = true;
                comboBox2.Enabled = checkBox1.Checked;

                ProductRepository repos = new ProductRepository();

                try
                {
                    var name = comboBox1.SelectedItem.ToString().Substring(comboBox1.SelectedItem.ToString().IndexOf("\""));
                    var data = repos.GetProductConteinerDataSource().
                    Where(elem => elem.Name.Contains(name) & elem.Amount != 0 & elem.Weight <= 3).
                    Select(element => $"{element.Name} {element.Weight}").ToList();

                    comboBox2.DataSource = (data.Count == 0) ? null : data;
                }
                catch (Exception)
                {
                    comboBox2.DataSource = null;
                }
            }
            else
            {
                this.Height -= 90;
                checkBox2.Location = checkBox2Position1;
                label5.Location = label5Position1;
                panel1.Location = new Point(4, 104);
                panel4.Visible = panel4.Enabled = false;
            }
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
                    double weightForRemaking = 0;
                    if (checkBox1.Checked)
                    {
                        foreach (var elementOfRemaking in listBox1.Items)
                        {
                            var productConteiner = productRepository.GetProductConteinerDataSource().
                            First(element => elementOfRemaking.ToString().Contains($"{element.Name} {element.Weight}"));

                            var arrayOfWords = elementOfRemaking.ToString().Split(' ');

                            int amount = Int32.Parse(arrayOfWords[0]);
                            weightForRemaking += productConteiner.Weight * amount;
                            var product = productRepository.GetDataSource().First(element => element.Name == comboBox1.SelectedItem.ToString());
                            var conteiner = conteinerRepository.GetDataSource().First(elem => elem.ProductId == product.Id & elem.Weight == productConteiner.Weight);

                            conteinerRepository.Remove(conteiner.Id, amount);

                            comboBox2.DataSource = productRepository.GetProductConteinerDataSource().
                            Where(elem => elem.Name.Contains(product.Name) & elem.Amount != 0 & elem.Weight <= 3).
                            Select(element => $"{element.Name} {element.Weight}").ToList();
                        }

                        if(weightForRemaking > weight * Decimal.ToInt32(numericUpDown1.Value))
                        {
                            double overWeightForRemaking = weightForRemaking - weight * Decimal.ToInt32(numericUpDown1.Value);

                            int idProduct = productRepository.GetDataSource()
                                .First(element => element.Name == comboBox1.SelectedItem.ToString()).Id;
                            InformationAboutOverweight form6 = new InformationAboutOverweight(overWeightForRemaking, idProduct);
                            form6.ShowDialog();
                        }
                    }

                    int id = productRepository.GetDataSource()
                        .First(element => element.Name == comboBox1.SelectedItem.ToString()).Id;
                    conteinerRepository.Create(
                        new Conteiner(id, weight, Decimal.ToInt32(numericUpDown1.Value)),
                        weightForRemaking,
                        checkBox2.Checked
                        );
                    listBox1.Items.Clear();
                    comboBox1.DataSource = productRepository.GetDataSource();
                    this.ComboBox1_SelectedIndexChanged(null, null);
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
            catch (Exception exception)
            {
                label5.ForeColor = System.Drawing.Color.Red;
                label5.Text = exception.Message;
            }
        }

        private void Add_MouseMove(object sender, MouseEventArgs e)
        {
            label5.ForeColor = System.Drawing.Color.Black;
            label5.Text = @"Інформація про додавання.";
        }
    }
}
