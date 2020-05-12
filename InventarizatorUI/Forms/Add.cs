using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories;
using InventarizatorLI.Repositories.TableJoin;
using System.Collections.Generic;

namespace InventarizatorUI
{
    public partial class Add : Form
    {
        Point panel1Position1, panel1Position2, label5Position1, label5Position2, checkBox2Position1, checkBox2Position2;
        public delegate void Upd();
        private event Upd updateInformation;
        private List<ProductConteiner> conteiners;
        private ProductConteiner elementForRemaking;
        private ProductConteiner defProductForRemaking;


        public Add(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            conteiners = new List<ProductConteiner>();
            InitializeComponent();
            Height -= 90;
            ProductRepository repos = new ProductRepository();
            comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
            checkBox1.Checked = false;
            comboBox2.Enabled = false;
            panel1Position1 = panel1.Location = new Point(9, 106);
            panel1Position2 = panel2.Location = new Point(9, 60);
            //label5Position1 = label5.Location = new Point(127, 84);
            //label5Position2 = label2.Location;
            //label5Position2.Y += 27;
            checkBox2Position1 = new Point(17, 84);
            checkBox2Position2 = new Point(17, 178);
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

                    for (int index = 0; index < conteiners.Count(); index++)
                    {
                        if (conteiners[index].Name == elementForRemaking.Name & conteiners[index].Weight == elementForRemaking.Weight)
                        {
                            if ((conteiners[index].Amount + elementForRemaking.Amount) > Int32.Parse(label7.Text))
                            {
                                MessageBox.Show(@"Кіликість продукту занадто велика.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                                conteiners[index].Amount += elementForRemaking.Amount;
                            wasNotFound = false;
                        }
                        listBox1.Items.Add(conteiners[index].ToString());
                    }
                    if (wasNotFound)
                    {
                        conteiners.Add(elementForRemaking);
                        listBox1.Items.Add(elementForRemaking.ToString());
                    }
                }
            }
            catch(Exception ex)
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
                conteiners.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            catch(ArgumentOutOfRangeException)
            { }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            comboBox2.Text = "";
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                ProductRepository repos = new ProductRepository();
                comboBox1.DataSource = repos.GetDataSource().Select(element => element.Name).ToList();
                label1.Text = @"Назва:";
                panel1.Location = panel1Position1;
                //label5.Location = label5Position1;
                Height += 45;
                panel2.Visible = panel2.Enabled = true;
                panel3.Visible = panel3.Enabled = true;
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
                panel1.Location = panel1Position2;
                //label5.Location = label5Position2;
                Height -= 45;
                maskedTextBox1.Mask = @"000.00";
            }
            maskedTextBox1.Text = "";
            numericUpDown1.Value = 1;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            elementForRemaking = null;
            if (checkBox1.Checked)
            {
                this.Height += 90;
                panel1.Location = new Point(4, 200);
                //label5.Location = new Point(127, 179);
                panel4.Visible = panel4.Enabled = true;
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
                this.Height -= 90;
                //label5.Location = label5Position1;
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
                    double weight = Double.Parse(maskedTextBox1.Text);
                   
                    if (maskedTextBox1.Text == " ," || weight == 0)
                    {
                        throw new FormatException("Невідома вага продукції.");
                    }
                    ConteinerRepository conteinerRepository = new ConteinerRepository();
                    ProductRepository productRepository = new ProductRepository();
                    double weightForRemaking = 0;
                    if (checkBox1.Checked)
                    {
                        foreach (var elementOfRemaking in conteiners)
                        {

                            weightForRemaking += elementForRemaking.Weight * elementForRemaking.Amount;
                            var product = productRepository.GetDataSource()
                                .First(element => element.Name == elementForRemaking.Name);
                            var conteiner = conteinerRepository.GetDataSource()
                                .First(elem => elem.ProductId == product.Id & elem.Weight == elementForRemaking.Weight);

                            conteinerRepository.Remove(conteiner.Id, dateTimePicker1.Value, 3 ,elementForRemaking.Amount);

                            comboBox2.DataSource = productRepository.GetProductConteinerDataSource().Where(elem => elem.Amount != 0 & elem.Weight <= 6)
                                .Select(n => n.ToString()).ToList();
                        }

                        if(weightForRemaking > weight * Decimal.ToInt32(numericUpDown1.Value))
                        {
                            double overWeightForRemaking = weightForRemaking - weight * Decimal.ToInt32(numericUpDown1.Value);

                            int idProduct = productRepository.GetDataSource()
                                .First(element => element.Name == comboBox1.SelectedItem.ToString()).Id;
                            InformationAboutOverweight form6 = new InformationAboutOverweight(dateTimePicker1.Value, overWeightForRemaking, idProduct);
                            form6.ShowDialog();
                        }
                    }

                    int id = productRepository.GetDataSource()
                        .First(element => element.Name == comboBox1.SelectedItem.ToString()).Id;
                    conteinerRepository.Add(
                        new Conteiner(id, weight, Decimal.ToInt32(numericUpDown1.Value)),
                        dateTimePicker1.Value, 
                        weightForRemaking,
                        false
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
                    repository.Add(new Package(id, weight), dateTimePicker1.Value);
                }


                MessageBox.Show(@"Об'єкт було успішно додано.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updateInformation();
            }
            catch (FormatException)
            {

                MessageBox.Show(@"Некоректна вага продукту.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exception)
            {
                //TODO: Program thrown message on english when user try to add some product. Also check this problem with ingredient.

                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //TODO: Delete an event method.
        private void Add_MouseMove(object sender, MouseEventArgs e)
        {
        }
    }
}
