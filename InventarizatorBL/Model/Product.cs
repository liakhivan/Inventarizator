using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace InventarizatorBL.Model
{
    class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ComponentOfProduct> Component { get; set; }
        public virtual ICollection<Package> Packages { get; set; }

        public Product(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("The name of product can not be null", nameof(name));
            }
            else
                Name = name;
        }
    }
}
