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
            patch = textBox1.Text = AppDomain.CurrentDomain.BaseDirectory;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (patch != null)
            {
                var repository = new ConteinerRepository();
                repository.BackupData(patch);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            textBox1.Text = fbd.SelectedPath;
            patch = textBox1.Text + "\\";
        }
    }
}
