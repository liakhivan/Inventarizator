using System.Data.Entity;

namespace InventarizatorLI.Model
{
    public class StorageDbContext : DbContext
    {
        public StorageDbContext() : base("StorageDBConnection")
        {
        }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Conteiner> Conteiners { get; set; }
        public DbSet<IngredientsForProduct> IngredientsForProducts { get; set; }
        public DbSet<StatElement<Ingredient>> IngredientStatistics { get; set; }
        public DbSet<StatElement<Product>> ProductStatistics { get; set; }

    }
}
