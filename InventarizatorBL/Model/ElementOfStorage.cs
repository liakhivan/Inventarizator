using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace InventarizatorBL.Model
{
    class ElementOfStorage
    {
        [Key]
        public int Id { get; set; }
        public double Count { get; set; }

        public ElementOfStorage(double count)
        {
            if (count < 0 || count > 200)
                throw new ArgumentException("The count of element can not be below 0 or above 200", nameof(count));
            else
                Count = count;
        }
    }
}
