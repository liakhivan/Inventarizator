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
    public partial class ClearStatistics : Form
    {
        public ClearStatistics()
        {
            InitializeComponent();
        }
        private void ClearStatistics_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = dateTimePicker2.MaxDate = DateTime.Today;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    ProdStatisticsRepository prodStatistics = new ProdStatisticsRepository();
                    prodStatistics.Remove(dateTimePicker1.Value, dateTimePicker2.Value);
                }
                else
                {
                    IngredStatisticsRepository ingredStatistics = new IngredStatisticsRepository();
                    ingredStatistics.Remove(dateTimePicker1.Value, dateTimePicker2.Value);
                }
                label2.ForeColor = System.Drawing.Color.Green;
                label2.Text = @"Статистику очищено.";
            }
            catch(Exception excep)
            {

                label2.ForeColor = System.Drawing.Color.Red;
                label2.Text = excep.Message;
            }
        }

        private void Label1_MouseMove(object sender, MouseEventArgs e)
        {

            label2.ForeColor = System.Drawing.Color.Black;
            label2.Text = @"Очищення статистики.";
        }
    }
}
