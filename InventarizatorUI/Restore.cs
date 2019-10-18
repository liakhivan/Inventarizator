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
        public Restore()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.ShowDialog();
            patch = ofd.FileName;
            textBox1.Text = patch;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var repository = new ProductRepository();
            repository.RestoreData(patch);
        }
    }
}
