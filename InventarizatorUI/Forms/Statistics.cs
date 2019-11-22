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
            ProdStatisticsRepository prodStatistics = new ProdStatisticsRepository();
            dataGridView1.DataSource = prodStatistics.GetProductStatistics();
            RadioButton1_CheckedChanged(this, null);
            dateTimePicker2.MaxDate = DateTime.Today;
            chart1.ChartAreas.Add("Статистика.");
        }

        private void Filter()
        {
            chart1.Series[0].Points.Clear();
            chart1.Titles.Clear();
            if(radioButton1.Checked)
            {
                ProdStatisticsRepository prodStatistics = new ProdStatisticsRepository();
                var statistics = prodStatistics.GetProductStatistics();
                statistics = statistics.Where(element => element.Name.ToUpper().Contains(textBox1.Text.ToUpper())).ToList();
                if (comboBox1.SelectedItem.ToString() != "Все")
                    statistics = statistics.Where(element => element.TypeEvent == comboBox1.SelectedItem.ToString()).ToList();
                if(!checkBox1.Checked)
                    statistics = statistics.Where(element => (element.Date >= dateTimePicker1.Value && element.Date <= dateTimePicker2.Value)).ToList();

                var chartDataSource = statistics.GroupBy(i => i.Date).Select(g => new { Date = g.Key, Weight = g.Sum(i => i.Weight) }).ToList();
                chart1.Titles.Add("Статистика продуктів.");
                foreach (var element in chartDataSource)
                    chart1.Series[0].Points.AddXY(element.Date.ToString("dd.MM.yy"), element.Weight);
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
                    statistics = statistics.Where(element => (element.Date >= dateTimePicker1.Value && element.Date <= dateTimePicker2.Value)).ToList();

                var chartDataSource = statistics.GroupBy(i => i.Date).Select(g => new { Date = g.Key, Weight = g.Sum(i => i.Weight) });
                chart1.Titles.Add("Статистика інгредієнтів.");
                foreach (var element in chartDataSource)
                    chart1.Series[0].Points.AddXY(element.Date.ToString("dd.MM.yy"), element.Weight);
                dataGridView1.DataSource = statistics;
            }
            chart1.DataBind();
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
    }
}
