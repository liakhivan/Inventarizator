using InventarizatorLI.Model;
using InventarizatorLI.Repositories.TableJoin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarizatorLI.Repositories
{
    public class IngredStatisticsRepository : GenericRepository<IngredStatElement>
    {
        //TODO: { 0, "Додано" }, { 1, "Списано вручну" }, { 2, "Списано" }
        Dictionary<int, string> typesEvents = new Dictionary<int, string>() { { 0, "Додано" }, { 1, "Списано вручну" }, { 2, "Списано" } };
        public Dictionary<int, string> TypeEvents { get { return typesEvents; } }
        public override List<IngredStatElement> GetDataSource()
        {
            using (var context = new StorageDbContext())
            {
                return context.IngredientStatistics.ToList<IngredStatElement>();
            }
        }

        public void Add(int ingredientId, int typeEvent, double weight, DateTime dateTime)
        {
            using (var context = new StorageDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.IngredientStatistics.Add(
                            new IngredStatElement(ingredientId, typeEvent, weight, dateTime)
                            );
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
        public void Remove(DateTime date1, DateTime date2)
        {
            using (var context = new StorageDbContext())
            {
                try
                {
                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.Configuration.AutoDetectChangesEnabled = false;
                    var listOfStatistics = context.IngredientStatistics.Where(element => element.Date <= date2 && element.Date >= date1);
                    foreach (var someConteiner in listOfStatistics)
                    {
                        context.IngredientStatistics.Attach(someConteiner);
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

        public List<IngredientStatistics> GetIngredientStatistics()
        {
            using (var context = new StorageDbContext())
            {
                var statistics = context.IngredientStatistics.ToList<IngredStatElement>();
                var ingredients = context.Ingredients.ToList();

                var buffStatistics = statistics.Join(
                    ingredients,
                    statElement => statElement.IdIngredient,
                    ingredient => ingredient.Id,
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
                    (statElement, typeEvent) => new IngredientStatistics()
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
