using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using InventarizatorLI.Model;

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

        public void Delete(Product product)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                try
                {
                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.Configuration.AutoDetectChangesEnabled = false;
                    var recept = context.IngredientsForProducts.Where(element =>
                        element.ProductId == product.Id);
                    foreach (var elementOfRecept in recept)
                    {
                        context.IngredientsForProducts.Attach(elementOfRecept);
                        context.Entry(elementOfRecept).State = EntityState.Deleted;
                    }
                    context.ChangeTracker.DetectChanges();
                    context.SaveChanges();
                }
                finally
                {
                    context.Configuration.ValidateOnSaveEnabled = true;
                }
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
