 using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace InventarizatorLI.Model
{
    public class Conteiner
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double Weight { get; set; }
        public int Amount { get; set; }


        public Conteiner(int productId, double weight, int amount)
        {
            if (productId > 0)
                ProductId = productId;
            else
                throw new ArgumentException("Id of product can't equals zero or below it.", nameof(productId));

            if (weight > 0)
                Weight = weight;
            else
                throw new ArgumentException("Weight of product in conteiner can't equals zero or below it.", nameof(weight));
 
            if (amount > 0)
                Amount = amount;
            else
                throw new ArgumentException("amount of conteiner can't equals zero or below it.", nameof(amount));
        }
    }
}
