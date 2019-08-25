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
        public double Weight { get; set; }

        public IngredientsForProduct(Product product, Ingredient ingredient, double weight)
        {
            Ingredient = ingredient ?? throw new ArgumentNullException("Ingredient can't be null.", nameof(ingredient));
            Product = product ?? throw new ArgumentNullException("Product can't be null.", nameof(product));
            IngredientId = ingredient.Id;
            ProductId = product.Id;
            if (weight <= 0 | weight > 9)
                throw new ArgumentException("Weight can't be above 9 kg or belowe 0.001 kg.", nameof(weight));
            else
                Weight = weight;
        }
    }
}
