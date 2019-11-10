using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarizatorLI.Model
{
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
