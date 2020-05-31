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
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (patch != null)
            {
                var repository = new ConteinerRepository();
                try
                {
                    repository.BackupData(patch);

                    MessageBox.Show($"Локальна копія створена.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
