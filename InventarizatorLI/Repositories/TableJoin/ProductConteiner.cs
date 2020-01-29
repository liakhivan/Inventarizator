using System;

namespace InventarizatorLI.Repositories.TableJoin
{
    public class ProductConteiner
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public int Amount { get; set; }

        public ProductConteiner()
        { }
        public ProductConteiner(string name, double weight, int amount)
        {
            if (name != null || name != "")
                Name = name;
            else
                throw new ArgumentException("Некоректна назва продукту");


            if (weight > 0)
                Weight = weight;
            else
                throw new ArgumentException("Некоректна вага продукту");


            if (amount > 0)
                Amount = amount;
            else
                throw new ArgumentException("Некоректна кількість продукту");
        }

        public override string ToString()
        {
            return (Amount + "  " + Name + "  " + Weight);
        }
    }
}
