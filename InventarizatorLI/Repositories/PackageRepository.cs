using System;
using System.Linq;
using System.Data.Entity;
using InventarizatorLI.Model;
using System.Collections.Generic;

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

        public List<Package> GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Packages.Load();
                return context.Packages.Local.ToList();
            }
        }

        public void Remove(int index, double amount)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        if (context.Packages.Find(index).Weight < amount)
                            throw new ArgumentOutOfRangeException();
                        if ((context.Packages.Find(index).Weight -= amount) == 0)
                        {
                            var somePackage = context.Packages.Find(index);
                            context.Configuration.ValidateOnSaveEnabled = false;
                            context.Packages.Attach(somePackage);
                            context.Entry(somePackage).State = EntityState.Deleted;
                        }
                        context.ChangeTracker.DetectChanges();
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw new ArgumentException();
                    }
                    finally
                    {
                        context.Configuration.ValidateOnSaveEnabled = true;
                    }
                }
            }
        }
    }
}
