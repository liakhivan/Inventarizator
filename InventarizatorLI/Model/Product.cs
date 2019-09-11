using System;
using System.ComponentModel.DataAnnotations;

namespace InventarizatorLI.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Product() { }
        public Product(string name)
        {
            Name = name ?? throw new ArgumentNullException($"The name of product can't be null.", nameof(name));
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
 