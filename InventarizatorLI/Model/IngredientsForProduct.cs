﻿using System;
using System.ComponentModel.DataAnnotations;

namespace InventarizatorLI.Model
{
    public class IngredientsForProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int IngredientId { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public double Weight { get; set; }
        public IngredientsForProduct()
        { }

        public IngredientsForProduct(int productId, int ingredientId, double weight)
        {
            if (ingredientId <= 0)
                throw new ArgumentNullException($"Ingredient can't be null.");
            if (productId <= 0)
                throw new ArgumentNullException($"Product can't be null.");
            IngredientId = ingredientId;
            ProductId = productId;
            if (weight <= 0 | weight > 9)
                throw new ArgumentException("Weight can't be above 9 kg or belowe 0.001 kg.", nameof(weight));
            else
                Weight = weight;
        }
        public IngredientsForProduct(Product product, Ingredient ingredient, double weight)
        {
            if (ingredient == null)
                throw new ArgumentNullException($"Ingredient can't be null.", nameof(ingredient));
            if (product == null)
                throw new ArgumentNullException($"Product can't be null.", nameof(product));
            IngredientId = ingredient.Id;
            ProductId = product.Id;
            if (weight <= 0 | weight > 9)
                throw new ArgumentException("Weight can't be above 9 kg or belowe 0.001 kg.", nameof(weight));
            else
                Weight = weight;
        }
    }
}
