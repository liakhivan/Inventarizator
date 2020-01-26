using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories.TableJoin;

namespace InventarizatorLI.Repositories
{
    public class ProdStatisticsRepository : GenericRepository<ProdStatElement>
    {
        //TODO: Продукти: { 0, "Додано" }, { 1, "Списано вручну" }, { 2, "Списано" }, { 3, "Перероблено" } }
        Dictionary<int, string> typesEvents = new Dictionary<int, string>() { {0, "Додано" }, {1, "Списано вручну" }, { 2, "Списано" }, {3, "Перероблено" } };
        public Dictionary<int, string> TypeEvents { get { return typesEvents; } }
        public override List<ProdStatElement> GetDataSource()
        {
            using(var context = new StorageDbContext())
            {
                return context.ProductStatistics.ToList();
            }
        }

        public void Add(int productId, int typeEvent, double weight, DateTime dateTime)
        {
            using(var context = new StorageDbContext())
            {
                using(var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.ProductStatistics.Add(
                            new ProdStatElement(productId, typeEvent, weight, dateTime)
                            );
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public void Remove(DateTime date1, DateTime date2)
        {
            using (var context = new StorageDbContext())
            {
                try
                {
                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.Configuration.AutoDetectChangesEnabled = false;
                    var listOfStatistics = context.ProductStatistics.Where(element => element.Date <= date2 && element.Date >= date1).ToList();
                    foreach (var someConteiner in listOfStatistics)
                    {
                        context.ProductStatistics.Attach(someConteiner);
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
        public List<ProductStatistics> GetProductStatistics()
        {
            using (var context = new StorageDbContext())
            {
                var statistics = context.ProductStatistics.ToList<ProdStatElement>();
                var products = context.Products.ToList();
                
                var buffStatistics = statistics.Join(
                    products,
                    statElement => statElement.IdProduct,
                    product => product.Id,
                    (statElement, product) => new
                    {
                        product.Name,
                        statElement.TypeEvent,
                        statElement.Weight,
                        statElement.Date
                    }
                    ).ToList();

                var newStatistics = buffStatistics.Join(
                    typesEvents,
                    statElement => statElement.TypeEvent,
                    typeEvent => typeEvent.Key,
                    (statElement, typeEvent) => new ProductStatistics()
                    {
                        Name = statElement.Name,
                        TypeEvent = typeEvent.Value,
                        Weight = statElement.Weight,
                        Date = statElement.Date
                    }
                    );
                return newStatistics.ToList();
            }
        }
    }
}
