using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace InventarizatorLI.Model
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //public Dictionary<Product, double> Products { get; set; }
        //public List<IngredientsForProduct> IngredientsForProducts { get; set; }
        public Ingredient() => Name = null;
        public Ingredient(string name, double weight)
        {
            // Products = new Dictionary<Product, double>();
            Name = name ?? throw new ArgumentNullException("The name of Ingredient can't be null.", nameof(name));
        }
    }
}
