using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace InventarizatorLI.Model
{
    public class Package
    {
        public int Id { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public double Weight { get; set; }

        public Package (int ingredientId, double weight)
        {
            if (ingredientId > 0)
                IngredientId = IngredientId;
            else
                throw new ArgumentException("Id of product can't equals zero or below it.", nameof(IngredientId));

            if (weight > 0)
                Weight = weight;
            else
                throw new ArgumentException("amount of conteiner can't equals zero or below it.", nameof(weight));
        }
    }
}
