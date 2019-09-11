using System;
using System.Linq;
using System.Data.Entity; 
using System.ComponentModel;
using InventarizatorLI.Model;

namespace InventarizatorLI.Repositories
{
    public class ConteinerRepository : IConteinerRepository
    {
        public void Create(Conteiner newConteiner)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Conteiner dbconteiner = context.Conteiners.FirstOrDefault(n => 
                             n.ProductId == newConteiner.ProductId & n.Weight == newConteiner.Weight);
                        if (dbconteiner != null)
                            dbconteiner.Amount += newConteiner.Amount;
                        else
                        {
                            context.Conteiners.Add(newConteiner);
                        }

                        context.SaveChanges();
                        var recept =
                            context.IngredientsForProducts.Where(element =>
                                element.ProductId == newConteiner.ProductId);
                        foreach (var oneIngredientOfRecept in recept)
                        {
                            foreach (var onePackage in context.Packages)
                            {
                                if (oneIngredientOfRecept.IngredientId == onePackage.IngredientId)
                                {
                                    var weight = oneIngredientOfRecept.Weight * newConteiner.Weight *
                                                 newConteiner.Amount;
                                    if (weight <= onePackage.Weight)
                                        onePackage.Weight -= weight;
                                    else
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
        public void Remove(int index, int amount)
        {
        }

        public void DeleteAll(Product product)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                try
                {
                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.Configuration.AutoDetectChangesEnabled = false;
                    var listOfConteiner = context.IngredientsForProducts.Where(element =>
                        element.ProductId == product.Id);
                    foreach (var someConteiner in listOfConteiner)
                    {
                        context.IngredientsForProducts.Attach(someConteiner);
                        context.Entry(someConteiner).State = EntityState.Deleted;
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

        public void DeleteByWeight(Conteiner oneConteiner)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                try
                {
                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.Configuration.AutoDetectChangesEnabled = false;
                    var listOfConteiner = context.IngredientsForProducts.Where(element =>
                        element.ProductId == oneConteiner.ProductId & element.Weight == oneConteiner.Weight);
                    foreach (var someConteiner in listOfConteiner)
                    {
                        context.IngredientsForProducts.Attach(someConteiner);
                        context.Entry(someConteiner).State = EntityState.Deleted;
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
        public void Update()
        {
            throw new NotImplementedException();
        }

        public Conteiner GetById(int index)
        {
            Conteiner elem;
            using (StorageDbContext context = new StorageDbContext())
            {
                elem = context.Conteiners.Find(index);
            }
            return elem;
        }

        BindingList<Conteiner> IGenericRepository<Conteiner, int>.GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Conteiners.Load();
                return context.Conteiners.Local.ToBindingList();
            }
        }
    }
}
