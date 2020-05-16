using System;
using System.Linq;
using System.Data.Entity; 
using InventarizatorLI.Model;
using System.Collections.Generic;
using InventarizatorLI.Repositories.TableJoin;

namespace InventarizatorLI.Repositories
{
    public class ConteinerRepository : GenericRepository<Conteiner>
    {
        public void Add(Conteiner newConteiner, DateTime dateAdd, double remake = 0, bool washer = false)
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
                            context.Conteiners.Add(newConteiner);

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
                                    //TODO: якщо треба збирати статистику списаних по рецепту інгредієнтів, то тут добавити додавання статистики
                                    else
                                        throw new ArgumentException();
                                    amountOfDontRemovedIngredients--;
                                }
                            }
                        }
                        if (amountOfDontRemovedIngredients != 0)
                            throw new ArgumentException();

                        var prodStat = new ProdStatisticsRepository();
                        prodStat.Add(newConteiner.ProductId, 0, newConteiner.Weight * newConteiner.Amount, dateAdd);
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


        public void AddRange(List<Conteiner> newConteiners, DateTime dateAdd, List<ProductConteiner> remakeCollection, bool washer = false)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        ProductRepository productRepository = new ProductRepository();

                        ConteinerRepository conteinerRepository = new ConteinerRepository();

                        foreach (var elementOfRemaking in remakeCollection)
                        {

                            var product = productRepository.GetDataSource()
                                .First(element => element.Name == elementOfRemaking.Name);

                            var conteiner = conteinerRepository.GetDataSource()
                                .First(elem => elem.ProductId == product.Id & elem.Weight == elementOfRemaking.Weight);

                            conteinerRepository.Remove(conteiner.Id, dateAdd.Date, 3, elementOfRemaking.Amount);
                        }





                        //Conteiner dbconteiner = context.Conteiners.FirstOrDefault(n =>
                        //     n.ProductId == newConteiner.ProductId & n.Weight == newConteiner.Weight);
                        //if (dbconteiner != null)
                        //    dbconteiner.Amount += newConteiner.Amount;
                        //else
                        //    context.Conteiners.Add(newConteiner);

                        //context.SaveChanges();
                        //double weightNewConteiner = newConteiner.Weight;
                        //if (remake != 0)
                        //    weightNewConteiner -= remake;

                        //if (washer)
                        //    weightNewConteiner += 0.6;

                        //var recept = context.IngredientsForProducts.
                        //    Where(element => element.ProductId == newConteiner.ProductId);
                        //int amountOfDontRemovedIngredients = recept.Count();
                        //foreach (var oneIngredientOfRecept in recept)
                        //{
                        //    foreach (var onePackage in context.Packages)
                        //    {
                        //        if (oneIngredientOfRecept.IngredientId == onePackage.IngredientId)
                        //        {
                        //            var weight = oneIngredientOfRecept.Weight * weightNewConteiner *
                        //                         newConteiner.Amount;
                        //            if (weight <= onePackage.Weight)
                        //                onePackage.Weight -= weight;
                        //            //TODO: якщо треба збирати статистику списаних по рецепту інгредієнтів, то тут добавити додавання статистики
                        //            else
                        //                throw new ArgumentException();
                        //            amountOfDontRemovedIngredients--;
                        //        }
                        //    }
                        //}
                        //if (amountOfDontRemovedIngredients != 0)
                        //    throw new ArgumentException();

                        //var prodStat = new ProdStatisticsRepository();
                        //prodStat.Add(newConteiner.ProductId, 0, newConteiner.Weight * newConteiner.Amount, dateAdd);
                        //context.SaveChanges();
                        //transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw new ArgumentException("Немає/недостатньо інгредієнтів.");
                    }
                }
            }
        }


        public void Remove(int index, DateTime dateRemove, int typeEvent, int amount = 1)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        context.Conteiners.Find(index).Amount -= amount;
                        var res = context.Conteiners.Find(index);
                        if (res.Amount < 0) throw new ArgumentOutOfRangeException();
                        if (res.Amount == 0)
                        {
                            var someConteiner = context.Conteiners.Find(index);
                            context.Configuration.ValidateOnSaveEnabled = false;
                            context.Conteiners.Attach(someConteiner);
                            context.Entry(someConteiner).State = EntityState.Deleted;
                        }
                        context.ProductStatistics.Add(new ProdStatElement(res.ProductId, typeEvent, res.Weight * amount, dateRemove));
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

        public override List<Conteiner> GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Conteiners.Load();
                return context.Conteiners.Local.ToList();
            }
        }
    }
}
