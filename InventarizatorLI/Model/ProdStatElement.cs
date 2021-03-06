﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarizatorLI.Model
{
    [Table("ProductStatistic")]
    public class ProdStatElement
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public virtual Product Product { get; set; }
        public int TypeEvent { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }

        public ProdStatElement()
        { }
        public ProdStatElement(int idProduct, int typeEvent, double weight)
        {
            IdProduct = (idProduct >= 0) ? idProduct : throw new ArgumentException();
            TypeEvent = typeEvent;
            Weight = (weight > 0) ? weight : throw new ArgumentException();
            Date = DateTime.Today;
        }
        public ProdStatElement(int idProduct, int typeEvent, double weight, DateTime date)
        {
            IdProduct = (idProduct >= 0) ? idProduct : throw new ArgumentException();
            TypeEvent = typeEvent;
            Weight = (weight > 0) ? weight : throw new ArgumentException();
            Date = date;
        }
    }
}
