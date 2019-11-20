using System;
using System.Collections.Generic;
using System.Linq;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories.TableJoin;

namespace InventarizatorLI.Repositories
{
    public class ProdStatisticsRepository : GenericRepository<ProdStatElement>
    {
        //TODO: Продукти: { 0, "Додано" }, { 1, "Списано вручну" }, { 2, "Списано" }, { 4, "Перероблено" } }
        Dictionary<int, string> typesEvents = new Dictionary<int, string>() { {0, "Додано" }, {1, "Списано вручну" }, { 2, "Списано" }, {4, "Перероблено" } };
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
                    );

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
