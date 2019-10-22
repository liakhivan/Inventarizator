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

namespace InventarizatorUI
{
    public partial class Restore : Form
    {
        string patch;
        public delegate void Upd();
        private event Upd updateInformation;
        public Restore(Upd updateEvent)
        {
            InitializeComponent();
            updateInformation += updateEvent;
            label3.Text = @"Інформація про відновлення.";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "SQL SERVER database backup files|*.bak";
            ofd.Title = "Database restore";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
                button2.Enabled = true;
                patch = ofd.FileName;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            label4.Visible = true;
            var repository = new ProductRepository();
            try
            {
                repository.RestoreData(patch);
                label3.ForeColor = System.Drawing.Color.Green;
                label3.Text = @"Дані успішно відновлені.";
            }
            catch(Exception exception)
            {
                label3.ForeColor = System.Drawing.Color.Red;
                label3.Text = @"Сталася помилка.";
                MessageBox.Show(exception.Message);
            }
            finally
            {

                label4.Visible = false;
            }
            updateInformation();
        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void Label3_MouseMove(object sender, MouseEventArgs e)
        {
            label3.ForeColor = System.Drawing.Color.Black;
            label3.Text = @"Інформація про відновлення.";
        }
    }
}
