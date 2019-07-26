using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace InventarizatorBL.Model
{
    class ComponentOfProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int IngredientId { get; set; }
        public double AmountOfIngredient { get; set; }

        public virtual Product Product { get; set; }
        public virtual Ingredient Ingredient { get; set; }

        public ComponentOfProduct(int productId, int ingredientId, double amount)
        {
            ProductId = productId;
            IngredientId = ingredientId;
            AmountOfIngredient = amount;
        }
    }
}
