using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace InventarizatorBL.Model
{
    class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
