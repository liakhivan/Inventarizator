using System;
using System.Data.Entity;
using InventarizatorBL.Model;

namespace InventarizatorBL
{
    class StorageDbContext : DbContext
    {
        public StorageDbContext() : base("DbConnection")
        { }

        public DbSet<ElementOfStorage> Storage { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<ComponentOfProduct> Components { get; set; }
    }
}
