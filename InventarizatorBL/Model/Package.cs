using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace InventarizatorBL.Model
{
    class Package
    {
        [Key]
        public int Id { get; set; }
        public double Weight { get; set; }
        public Package(double weight)
        {
            if (weight <= 0 || weight > 100)
                throw new ArgumentException("The weight of package can not be below 0 or above 200", nameof(weight));
            else
                Weight = weight;
        }
    }
}
