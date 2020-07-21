using InventarizatorLI.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventarizatorUI.Forms
{
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SecurityRepository securityRepository = new SecurityRepository();

            try
            {
                bool isPrewPasswordCorrect = securityRepository.VerifyPassword(textBox1.Text);

                if (!isPrewPasswordCorrect)
                {
                    throw new Exception("Вказано неправильний пароль.");
                }
                if (textBox2.Text == textBox3.Text)
                {
                    throw new Exception("Новий пароль не збігається з вказаним.");
                }

                securityRepository.Edit(textBox3.Text);

                MessageBox.Show(@"Заміна пароля пройшла успішно.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                textBox2.Focus();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox3.Focus();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
            }
        }
    }
}
