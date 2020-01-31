using InventarizatorLI.Repositories;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace InventarizatorUI.Forms
{
    public partial class Statistics : Form
    {
        public Statistics()
        {
            InitializeComponent();
        }

        private void Filter()
        {
            DateTime date1 = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);

            DateTime date2 = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);

            if (radioButton1.Checked)
            {
                ProdStatisticsRepository prodStatistics = new ProdStatisticsRepository();
                var statistics = prodStatistics.GetProductStatistics();
                statistics = statistics.Where(element => element.Name.ToUpper().Contains(textBox1.Text.ToUpper())).ToList();
                if (comboBox1.SelectedItem.ToString() != "Все")
                    statistics = statistics.Where(element => element.TypeEvent == comboBox1.SelectedItem.ToString()).ToList();
                if(!checkBox1.Checked)
                    statistics = statistics.Where(element => (element.Date >= date1.Date && element.Date <= date2.Date)).ToList();
                dataGridView1.DataSource = statistics;
            }
            else
            {
                IngredStatisticsRepository ingredStatistics = new IngredStatisticsRepository();
                var statistics = ingredStatistics.GetIngredientStatistics();
                statistics = statistics.Where(element => element.Name.ToUpper().Contains(textBox1.Text.ToUpper())).ToList();
                if (comboBox1.SelectedItem.ToString() != "Все")
                    statistics = statistics.Where(element => element.TypeEvent == comboBox1.SelectedItem.ToString()).ToList();
                if (!checkBox1.Checked)
                    statistics = statistics.Where(element => (element.Date >= date1.Date && element.Date <= date2.Date)).ToList();
                dataGridView1.DataSource = statistics;
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            List<string> statisticList = new List<string>();
            statisticList.Add("Все");
            if(radioButton1.Checked)
            {
                ProdStatisticsRepository prodStatistics = new ProdStatisticsRepository();
                statisticList.AddRange(prodStatistics.TypeEvents.Values);
            }
            else
            {
                IngredStatisticsRepository ingredStatistics = new IngredStatisticsRepository();
                statisticList.AddRange(ingredStatistics.TypeEvents.Values);
            }
            checkBox1.Checked = true;
            comboBox1.DataSource = statisticList;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void ComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                panel3.Enabled = false;
            else
                panel3.Enabled = true;
            Filter();
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void Statistics_Load(object sender, EventArgs e)
        {
            ProdStatisticsRepository prodStatistics = new ProdStatisticsRepository();
            dataGridView1.DataSource = prodStatistics.GetProductStatistics(); 
            dataGridView1.Columns["Weight"].DefaultCellStyle.Format = "#0.00";
            dataGridView1.AutoResizeColumns();
            RadioButton1_CheckedChanged(this, null);
            dateTimePicker1.MaxDate = DateTime.Today; 
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.MaxDate = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
        }
    }
}
