using System;
using System.ComponentModel.DataAnnotations;

namespace InventarizatorLI.Model
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Ingredient() => Name = null;

        public Ingredient(string name)
        {
            Name = name ?? throw new ArgumentNullException($"The name of Ingredient can't be null.", nameof(name));
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
