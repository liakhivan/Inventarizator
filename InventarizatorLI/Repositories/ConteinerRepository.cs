using System;
using System.Linq;
using System.Data.Entity; 
using InventarizatorLI.Model;
using InventarizatorLI.Comparators;

namespace InventarizatorLI.Repositories
{
    public class ConteinerRepository : IConteinerRepository
    {
        public StorageDbContext context = new StorageDbContext();
        public void Create(Conteiner conteiner)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    //Find conteiner with this characteristic
                    if (context.Conteiners.Contains(conteiner, new ConteinerEqualityComparer()))
                    {
                        //if conteiner was found, incremented Amount
                        Conteiner dbconteiner = context.Conteiners.Where(n => n.Weight == conteiner.Weight).FirstOrDefault();
                        dbconteiner.Amount++;
                    }
                    else
                    {
                        //Else create new conteiner
                        context.Conteiners.Add(conteiner);
                    }

                    //foreach (var oneIngredientOfRecept in conteiner.Product.Recept)
                    //{
                    //    //context.Ingredients.Find(oneIngredientOfRecept.Name).Weight
                    //    foreach (var oneIngredient in context.Ingredients)
                    //    {
                    //        if (oneIngredientOfRecept.Key.Id == oneIngredient.Id)
                    //        {
                    //            oneIngredient.Weight -= (oneIngredientOfRecept.Value * conteiner.Weight);
                    //        }
                    //    }
                    //}

                    context.SaveChanges();
                    transaction.Commit();
                } catch (Exception e)
                {
                    transaction.Rollback();
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
            return context.Conteiners.Find(index);
        }
    }
}
