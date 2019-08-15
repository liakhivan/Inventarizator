using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace InventarizatorLI.Model
{
    public class IngredientsForProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public int Weight { get; set; }
    }
}
