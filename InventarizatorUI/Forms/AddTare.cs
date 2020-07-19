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
    public partial class AddTare : Form
    {

        BindingSource bs;
        List<string> coll;

        public delegate void Upd();
        private event Upd updateInformation;

        public AddTare(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            InitializeComponent();

            TareRepository repos = new TareRepository();
            coll = repos.GetDataSource().Select(element => element.Name).ToList<string>();
            comboBox1.SelectedIndex = -1;
            label1.Text = @"Назва:";

            bs = new BindingSource();
            bs.DataSource = coll;
            comboBox1.DataSource = bs;
            comboBox1.SelectedItem = null;

            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            comboBox1.AutoCompleteMode = AutoCompleteMode.None;
            comboBox1.IntegralHeight = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                TareRepository tareRepository = new TareRepository();

                Tare tareForAdding = tareRepository.GetDataSource().First(n => n.Name == comboBox1.SelectedItem.ToString());
                tareForAdding.Amount = Decimal.ToInt32(numericUpDown1.Value);

                tareRepository.Add(tareForAdding);

                updateInformation();
                MessageBox.Show(@"Тару було успішно додано.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show($"Тару не вибрано.\nБудь ласка виберіть потрібну тару.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ;
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
                numericUpDown1.Focus();
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.Focus();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = 1;
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            string searchString = comboBox1.Text;
            Cursor prevCursor = this.Cursor;
            bs.DataSource = coll.Where(x => x.ToUpper().Contains(searchString.ToUpper())).ToList();
            if (bs.Count != 0)
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