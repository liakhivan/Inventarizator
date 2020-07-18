using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarizatorLI.Model
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Client()
        {}

        public Client(string name)
        {
            Name = name ?? throw new ArgumentNullException("Не вказано ім'я клієнта.");
        }
    }
}
