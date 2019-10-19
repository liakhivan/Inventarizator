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
    public partial class Backup : Form
    {
        string patch;
        public Backup()
        {
            InitializeComponent();
            label3.Text = @"Інформація про створення.";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (patch != null)
            {
                var repository = new ConteinerRepository();
                try
                {
                    repository.BackupData(patch);

                    label3.ForeColor = System.Drawing.Color.Green;
                    label3.Text = @"Локальна копія створена.";
                }
                catch (Exception exception)
                {
                    label3.ForeColor = System.Drawing.Color.Red;
                    label3.Text = exception.Message;
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;
                button2.Enabled = true;
                patch = textBox1.Text + "\\";
            }
        }

        private void Label3_MouseMove(object sender, MouseEventArgs e)
        {
            label3.ForeColor = System.Drawing.Color.Black;
            label3.Text = @"Інформація про створення.";
        }

        private void Backup_Load(object sender, EventArgs e)
        {

        }
    }
}
