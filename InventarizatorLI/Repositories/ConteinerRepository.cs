using System;
using System.Linq;
using System.Data.Entity; 
using System.ComponentModel;
using InventarizatorLI.Model;
using InventarizatorLI.Comparators;

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
                        if (context.Conteiners.Contains(newConteiner, new ConteinerEqualityComparer()))
                        {
                            Conteiner dbconteiner = context.Conteiners.Where(n => n.Weight == newConteiner.Weight).FirstOrDefault();
                            dbconteiner.Amount += newConteiner.Amount;
                        }
                        else
                        {
                            context.Conteiners.Add(newConteiner);
                        }

                        var recept = context.IngredientsForProducts.Where(element => element.ProductId == newConteiner.ProductId);
                        foreach (var oneIngredientOfRecept in recept)
                        {
                            foreach (var onePackage in context.Packages)
                            {
                                if (oneIngredientOfRecept.IngredientId == onePackage.IngredientId)
                                {
                                    onePackage.Weight -= (oneIngredientOfRecept.Weight * newConteiner.Weight * newConteiner.Amount);
                                }
                            }
                        }
                        context.SaveChanges();
                        transaction.Commit();
                    } catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        public void Remove(int index, int amount)
        {
            //context.Conteiners.
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
