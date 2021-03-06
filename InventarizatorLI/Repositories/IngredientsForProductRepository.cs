﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using InventarizatorLI.Model;

namespace InventarizatorLI.Repositories
{
    public class IngredientsForProductRepository : GenericRepository<IngredientsForProduct>
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

        public void Edit(IngredientsForProduct newIngredientsForProduct)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.IngredientsForProducts.First(n => n.Id == newIngredientsForProduct.Id).IngredientId = newIngredientsForProduct.IngredientId;

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public void Remove(Conteiner newConteiner)
        {
            using(var context = new StorageDbContext())
            {
                var recept =
                    context.IngredientsForProducts.Where(element =>
                        element.ProductId == newConteiner.ProductId);
                foreach (var oneIngredientOfRecept in recept)
                {
                    foreach (var onePackage in context.Packages)
                    {
                        if (oneIngredientOfRecept.IngredientId == onePackage.IngredientId)
                        {
                            var weight = oneIngredientOfRecept.Weight * newConteiner.Weight * newConteiner.Amount;
                            if (weight <= onePackage.Weight)
                                onePackage.Weight -= weight;
                            else
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
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
                    var recept = context.IngredientsForProducts.Where(element => element.ProductId == product.Id);
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

        public override List<IngredientsForProduct> GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.IngredientsForProducts.Load();
                return context.IngredientsForProducts.Local.ToList();
            }
        }
    }
}
