using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarizatorLI.Model
{
    public class ElementOfInvoice
    {
        public string Product { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }

        public ElementOfInvoice(string product, double weight, int count, double price)
        {
            Product = product;
            Weight = weight;
            Count = count;
            Price = price;
        }
    }
}
