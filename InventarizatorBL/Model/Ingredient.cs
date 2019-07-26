using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace InventarizatorBL.Model
{
    class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public Ingredient(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("The name of ingredient can not be null", nameof(name));
            }
            else
                Name = name;
        }
    }
}
