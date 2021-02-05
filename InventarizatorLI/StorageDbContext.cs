using System.Data.Entity;

namespace InventarizatorLI.Model
{
    class ContextInitializer : CreateDatabaseIfNotExists<StorageDbContext>
    {
        protected override void Seed(StorageDbContext db)
        {
            Security defSecurity = new Security();

            db.Securities.Add(defSecurity);
            db.SaveChanges();
        }
    }

    public class StorageDbContext : DbContext
    {

        public StorageDbContext() : base("StorageDBConnection")
        {
            Database.SetInitializer<StorageDbContext>(new ContextInitializer());
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Conteiner> Conteiners { get; set; }
        public DbSet<IngredientsForProduct> IngredientsForProducts { get; set; }
        public DbSet<IngredStatElement> IngredientStatistics { get; set; }
        public DbSet<ProdStatElement> ProductStatistics { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Tare> Tares { get; set; }
        public DbSet<Security> Securities { get; set; }
    }
}
