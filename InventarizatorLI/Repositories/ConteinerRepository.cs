using System;
using System.Linq;
using System.Data.Entity; 
using System.ComponentModel;
using InventarizatorLI.Model;
using System.Collections.Generic;

namespace InventarizatorLI.Repositories
{
    public class ConteinerRepository : IConteinerRepository
    {
        public void Create(Conteiner newConteiner, double remake = 0, bool washer = false)
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
                        double weightNewConteiner = newConteiner.Weight;
                        if (remake != 0)
                            weightNewConteiner -= remake;

                        if (washer)
                            weightNewConteiner += 0.6;

                        var recept = context.IngredientsForProducts.
                            Where(element => element.ProductId == newConteiner.ProductId);
                        int amountOfDontRemovedIngredients = recept.Count();
                        foreach (var oneIngredientOfRecept in recept)
                        {
                            foreach (var onePackage in context.Packages)
                            {
                                if (oneIngredientOfRecept.IngredientId == onePackage.IngredientId)
                                {
                                    var weight = oneIngredientOfRecept.Weight * weightNewConteiner *
                                                 newConteiner.Amount;
                                    if (weight <= onePackage.Weight)
                                        onePackage.Weight -= weight;
                                    else
                                        throw new ArgumentException();
                                    amountOfDontRemovedIngredients--;
                                }
                            }
                        }
                        if (amountOfDontRemovedIngredients != 0)
                            throw new ArgumentException();

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw new ArgumentException("Немає/недостатньо інгредієнтів.");
                    }
                }
            }
        }

        
        public void Remove(int index, int amount = 1)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        context.Conteiners.Find(index).Amount -= amount;
                        var res = context.Conteiners.Find(index).Amount;
                        if (res < 0) throw new ArgumentOutOfRangeException();
                        if (res == 0)
                        {
                            var someConteiner = context.Conteiners.Find(index);
                            context.Configuration.ValidateOnSaveEnabled = false;
                            context.Conteiners.Attach(someConteiner);
                            context.Entry(someConteiner).State = EntityState.Deleted;

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

        public List<Conteiner> GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Conteiners.Load();
                return context.Conteiners.Local.ToList();
            }
        }
    }
}
