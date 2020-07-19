using InventarizatorLI.Model;
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
    public partial class CreateTare : Form
    {
        public CreateTare()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TareRepository tareRepository = new TareRepository();

            try
            {
                Tare newTare = new Tare(textBox1.Text, 0);

                tareRepository.Create(newTare);


                MessageBox.Show("Операція успішно виконана.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
