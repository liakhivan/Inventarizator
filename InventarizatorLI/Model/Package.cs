using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

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
                IngredientId = ingredientId;
            else throw new ArgumentException("Package can't be empty.", nameof(IngredientId));

            if (weight > 0)
                Weight = weight;
            else
                throw new ArgumentException("Amount of conteiner can't equals zero or below it.", nameof(weight));
        }
    }
}
