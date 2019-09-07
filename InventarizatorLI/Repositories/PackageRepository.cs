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
                Package findPackage = context.Packages.Where(n => n.IngredientId == newPackage.IngredientId).FirstOrDefault();
                if(findPackage != null)
                    findPackage.Weight += newPackage.Weight;
                else
                    context.Packages.Add(newPackage);
                context.SaveChanges();
            }
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
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
