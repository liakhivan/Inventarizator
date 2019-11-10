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
        public string TypeEvent { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
    }
}
