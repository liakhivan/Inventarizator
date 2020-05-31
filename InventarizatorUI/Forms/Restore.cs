using InventarizatorLI.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            var repository = new ProductRepository();
            try
            {
                Thread threadForRestoreDataBase = new Thread(new ParameterizedThreadStart(repository.RestoreData));

                MessageBox.Show($"Відновлення даних може зайняти декілька хвилин.\nНе виконуйте жодних дій.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.WaitCursor;

                threadForRestoreDataBase.Start(patch);
                threadForRestoreDataBase.Join();

                this.Cursor = Cursors.Default;
                MessageBox.Show(@"Дані успішно відновлені.", "Sucsess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            updateInformation();
        }
    }
}
