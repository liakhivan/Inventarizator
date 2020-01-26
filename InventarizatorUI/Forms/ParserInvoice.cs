﻿using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using InventarizatorLI.Model;
using System.Collections.Generic;
using InventarizatorLI.Repositories;
using Excel = Microsoft.Office.Interop.Excel;

namespace InventarizatorUI.Forms
{
    public partial class ParserInvoice : Form
    {
        public delegate void Upd();
        private event Upd updateInformation;
        Dictionary<Conteiner, int> productCount;
        DateTime date;
        private const int startProductPosition = 30;
        Excel.Worksheet sheet;
        Excel.Application invoiceFile = new Excel.Application
        {
            Visible = false
        };

        public ParserInvoice(Upd eventUpdate)
        {
            updateInformation += eventUpdate;
            InitializeComponent();
            productCount = new Dictionary<Conteiner, int>();
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                string nameProduct;
                int count;
                double weight;

                ConteinerRepository conteinerRepository = new ConteinerRepository();
                ProductRepository productRepository = new ProductRepository();

                OpenFileDialog OPF = new OpenFileDialog();
                if (OPF.ShowDialog() == DialogResult.OK)
                {
                    string fileInvoice = OPF.FileName;
                // Відкриття файлу накладної.
                invoiceFile.Workbooks.Open(fileInvoice.ToString());
                }
                sheet = invoiceFile.Sheets[2];

                string dateString = sheet.Range["D1"].Value;
                var massDate = dateString.Split('.');

                date = new DateTime(Int32.Parse(massDate[2]), Int32.Parse(massDate[1]), Int32.Parse(massDate[0]));
                int i = 1;
                while ((string)(sheet.Range["A" + i].Value) != null)
                {
                    nameProduct = sheet.Range["A" + i].Value;
                    count = (int)sheet.Range["B" + i].Value;
                    weight = sheet.Range["C" + i].Value;
                    textBox1.Text += nameProduct + "   " + count + "   " + weight + "\r\n";
                    var product = productRepository.GetDataSource().FirstOrDefault(n => n.Name == nameProduct);
                    if (product == null)
                    {
                        throw new ArgumentException();
                    }
                    var conteiner = conteinerRepository.GetDataSource().FirstOrDefault(n => (n.ProductId == product.Id & n.Weight == weight));
                    if (conteiner == null)
                    {
                        throw new ArgumentNullException("");
                    }
                    productCount.Add(conteiner, count);
                    i++;
                }

                label1.ForeColor = System.Drawing.Color.Green;
                label1.Text = @"Накладна успішно відкрита.";
            }
            catch (Exception ex)
            {
                label1.ForeColor = System.Drawing.Color.Red;
                label1.Text = ex.Message;

                invoiceFile.Workbooks.Close();
                invoiceFile.Quit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConteinerRepository conteinerRepository = new ConteinerRepository();
            ProductRepository productRepository = new ProductRepository();
            //Заповнення накладної.

            try
            {
                int i = 1;
                foreach(var elem in productCount)
                {
                    conteinerRepository.Remove(elem.Key.Id, date, 2, elem.Value);
                }

                label1.ForeColor = System.Drawing.Color.Green;
                label1.Text = @"Продукти успішно списані.";
            }
            catch (Exception ex)
            {
                label1.ForeColor = System.Drawing.Color.Red;
                label1.Text = ex.Message;
            }
            finally
            {
                invoiceFile.Workbooks.Close();
                invoiceFile.Quit();
                updateInformation();
            }

        }
    }
}