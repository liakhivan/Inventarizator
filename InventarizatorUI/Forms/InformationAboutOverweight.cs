using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventarizatorLI.Repositories;
using InventarizatorLI.Model;

namespace InventarizatorUI
{
    public partial class InformationAboutOverweight : Form
    {
        int idProduct;
        double weight;
        DateTime dateAdd;
        public InformationAboutOverweight(DateTime dateAdd, double weight, int idProduct)
        {
            InitializeComponent();
            this.idProduct = idProduct;
            this.weight = weight;
            label1.Text += (" " + weight.ToString("0.00"));
            this.dateAdd = dateAdd;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var conteinerRepository = new ConteinerRepository();
            try
            {
                double weightNewConteiner = double.Parse(maskedTextBox1.Text);
                if (weight - weightNewConteiner < 0)
                    throw new ArgumentException();
                conteinerRepository.Add(new Conteiner(idProduct, weightNewConteiner, 1), dateAdd);
                if(weightNewConteiner - weight < 0)
                {
                    var statistic = new ProdStatisticsRepository();
                    statistic.Add(idProduct, 1, weightNewConteiner - weight, dateAdd);
                }
            }
            catch (FormatException) { }
            catch (ArgumentException) { }
            finally
            {
                this.Close();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
