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
    }
}
