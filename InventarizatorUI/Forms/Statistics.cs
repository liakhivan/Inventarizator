using InventarizatorLI.Repositories;
using System;
using System.Windows.Forms;
using System.Linq;

namespace InventarizatorUI.Forms
{
    public partial class Statistics : Form
    {
        public Statistics()
        {
            InitializeComponent();
            ProdStatisticsRepository prodStatistics = new ProdStatisticsRepository();
            dataGridView1.DataSource = prodStatistics.GetProductStatistics();
            chart1.DataSource = dataGridView1.DataSource;
            dateTimePicker2.MaxDate = DateTime.Today;
        }

        private void Filter()
        {
            if(radioButton1.Checked)
            {
                ProdStatisticsRepository prodStatistics = new ProdStatisticsRepository();
                var statistics = prodStatistics.GetProductStatistics();
                statistics = statistics.Where(element => element.Name.Contains(textBox1.Text) && 
                                            element.TypeEvent == comboBox1.SelectedItem.ToString() &&
                                            (element.Date >= dateTimePicker1.Value && element.Date <= dateTimePicker2.Value)).ToList();
                dataGridView1.DataSource = statistics;
            }
            else
            {
                IngredStatisticsRepository ingredStatistics = new IngredStatisticsRepository();
                var statistics = ingredStatistics.GetIngredientStatistics();
                statistics = statistics.Where(element => element.Name.Contains(textBox1.Text) &&
                                            element.TypeEvent == comboBox1.SelectedItem.ToString() &&
                                            (element.Date >= dateTimePicker1.Value && element.Date <= dateTimePicker2.Value)).ToList();
                dataGridView1.DataSource = statistics;
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                ProdStatisticsRepository prodStatistics = new ProdStatisticsRepository();
                dataGridView1.DataSource = prodStatistics.GetProductStatistics();
                comboBox1.DataSource = prodStatistics.TypeEvents.Values.ToList();
            }
            else
            {
                IngredStatisticsRepository ingredStatistics = new IngredStatisticsRepository();
                dataGridView1.DataSource = ingredStatistics.GetIngredientStatistics();
                comboBox1.DataSource = ingredStatistics.TypeEvents.Values.ToList();
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void DateTimePicker1_Enter(object sender, EventArgs e)
        {
            Filter();
        }
    }
}
