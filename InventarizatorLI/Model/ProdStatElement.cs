using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarizatorLI.Model
{
    [Table("ProductStatistics")]
    public class ProdStatElement
    {

        public int Id { get; set; }
        public int IdProduct { get; set; }
        public virtual Product Product { get; set; }
        public string TypeEvent { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
    }
}
