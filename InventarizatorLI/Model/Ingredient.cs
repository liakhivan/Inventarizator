using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarizatorLI.Model
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        //[Index(IsUnique = true)]
        public string Name { get; set; }
        public Ingredient() => Name = null;

        public Ingredient(string name)
        {
            Name = name ?? throw new ArgumentNullException("The name of Ingredient can't be null.", nameof(name));
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
