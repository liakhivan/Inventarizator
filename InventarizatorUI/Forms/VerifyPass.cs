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
    public partial class VerifyPass : Form
    {
        private bool isPassCorrect = false;
        public VerifyPass()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SecurityRepository securityRepository = new SecurityRepository();

            isPassCorrect = securityRepository.VerifyPassword(textBox1.Text.ToString());

            try
            {
                if (!isPassCorrect)
                {
                    throw new Exception("Введено неправильний пароль.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close(); 
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                throw new Exception("Пароль не підтверджено.");
            } 
            finally
            {
                this.Close();
            }
        }
    }
}
