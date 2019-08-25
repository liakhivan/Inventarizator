using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace InventarizatorLI.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Product() { }
        public Product(string name/*, Dictionary<Ingredient, double> recept*/ )
        {
            Name = name ?? throw new ArgumentNullException("The name of product can't be null.", nameof(name));
            // Ingredients = new Dictionary<Ingredient, double>();
            //Ingredients = recept ?? throw new ArgumentNullException("The recept of product can't be null.", nameof(recept));
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
 