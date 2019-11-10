using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarizatorLI.Model
{
    [Table("IngredientStatistics")]
    public class IngredStatElement
    {
        public int Id { get; set; }
        public int IdIngredient { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public int TypeEvent { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
        public IngredStatElement(int idIngredient, int typeEvent, double weight)
        {
            IdIngredient = (idIngredient >= 0) ? idIngredient : throw new ArgumentException();
            TypeEvent = typeEvent;
            Weight = (weight > 0) ? weight : throw new ArgumentException();
            Date = DateTime.Today;
        }
        public IngredStatElement(int idIngredient, int typeEvent, double weight, DateTime date)
        {
            IdIngredient = (idIngredient >= 0) ? idIngredient : throw new ArgumentException();
            TypeEvent = typeEvent;
            Weight = (weight > 0) ? weight : throw new ArgumentException();
            Date = date;
        }
    }
}
