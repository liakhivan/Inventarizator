using System;
using System.Linq;
using System.Data.Entity;
using System.ComponentModel;
using InventarizatorLI.Model;

namespace InventarizatorLI.Repositories
{
    public class PackageRepository : IPackageRepository
    {

        public void Create(Package newPackage)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                var findPackage = context.Packages.FirstOrDefault(n => n.IngredientId == newPackage.IngredientId);
                if(findPackage != null)
                    findPackage.Weight += newPackage.Weight;
                else
                    context.Packages.Add(newPackage);
                context.SaveChanges();
            }
        }

        public void Delete(Ingredient ingredient)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                try
                {
                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.Configuration.AutoDetectChangesEnabled = false;
                    var conteiner = context.Packages.FirstOrDefault(element =>
                        element.IngredientId == ingredient.Id);
                    if (conteiner != null)
                    {
                        context.Packages.Attach(conteiner);
                        context.Entry(conteiner).State = EntityState.Deleted;
                        context.ChangeTracker.DetectChanges();
                        context.SaveChanges();
                    }
                }
                finally
                {
                    context.Configuration.ValidateOnSaveEnabled = true;
                }
            }
        }

        public Package GetById(int index)
        {
            Package elem;
            using (StorageDbContext context = new StorageDbContext())
            {
                elem = context.Packages.Find(index);
            }
            return elem;
        }

        public BindingList<Package> GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Packages.Load();
                return context.Packages.Local.ToBindingList();
            }
        }

        public void Remove(int index, int amount)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
