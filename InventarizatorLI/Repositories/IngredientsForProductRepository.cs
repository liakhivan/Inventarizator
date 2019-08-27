using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventarizatorLI.Model;
using InventarizatorLI;

namespace InventarizatorLI.Repositories
{
    class IngredientsForProductRepository : IIngredientsForProductRepository
    {
        public void Create(List<IngredientsForProduct> newIngredientsForProduct)
        {
            using(StorageDbContext context = new StorageDbContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                foreach (var element in newIngredientsForProduct ?? throw new ArgumentNullException())
                    context.IngredientsForProducts.Add(element);
                context.ChangeTracker.DetectChanges();
                context.SaveChanges();
            }
        }

       
        public IngredientsForProduct GetById(int index)
        {
            throw new NotImplementedException();
        }

        public BindingList<IngredientsForProduct> GetDataSource()
        {
            throw new NotImplementedException();
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
