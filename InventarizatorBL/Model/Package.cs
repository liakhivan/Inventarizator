using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace InventarizatorBL.Model
{
    class Package
    {
        [Key]
        public int Id { get; set; }
        public double Weight { get; set; }
    }
}
