using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarizatorLI.Model
{
    public class Tare
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }

        public Tare()
        { }

        public Tare(string name, int amount)
        {
            if (amount < 0)
                throw new Exception("Кількість тари не коректна.");

            Name = name ?? throw new ArgumentNullException("Не вказано назву тари.");
            Amount = amount;
        }
    }
}
