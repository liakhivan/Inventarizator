using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using InventarizatorLI.Model;

namespace InventarizatorUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var dbcontext = new StorageDbContext())
            {
                BindingSource bs = new BindingSource();
                dbcontext.Ingredients.Where(x => x.Name == "Milk").Load();
                bs.DataSource = dbcontext.Ingredients.Local.ToBindingList();
                dataGridView1.DataSource = bs;
            }
        }
    }
}
