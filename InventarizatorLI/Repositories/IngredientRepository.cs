using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories.TableJoin;

namespace InventarizatorLI.Repositories
{
    public class IngredientRepository : GenericRepository<Ingredient>
    {
        public void Create(Ingredient newIngredient)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                var ingredient = context.Ingredients.FirstOrDefault(element => element.Name == newIngredient.Name);
                if (ingredient == null)
                {
                    context.Ingredients.Add(newIngredient);
                    context.SaveChanges();
                }
                else throw new ArgumentException($"Цей інгредієнт вже існує.");
            }
        }

        public void Edit(Ingredient newIngredient)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Ingredient ingredientForChanging = context.Ingredients.First(n => n.Id == newIngredient.Id);

                        ingredientForChanging.Name = newIngredient.Name;

                        context.ChangeTracker.DetectChanges();
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw new InvalidOperationException("Помилка редагування.");
                    }
                    finally
                    {
                        context.Configuration.ValidateOnSaveEnabled = true;
                    }
                }
            }
        }

        public void Delete(Ingredient ingredient)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        PackageRepository package = new PackageRepository();
                        ProductRepository productRepository = new ProductRepository();
                        IngredientsForProductRepository ingredientsForProductRepository = new IngredientsForProductRepository();

                        package.Delete(ingredient);

                        var allReceiptsWithEntryIngredients = ingredientsForProductRepository.GetDataSource().Where(n => n.IngredientId == ingredient.Id).ToList();
                        if (allReceiptsWithEntryIngredients != null)
                        {
                            foreach (var oneIngredientForProductInReceipt in allReceiptsWithEntryIngredients)
                            {
                                Product productWithEntryIngredientInReceipt = productRepository.GetDataSource().First(n => n.Id == oneIngredientForProductInReceipt.ProductId);

                                productRepository.Delete(productWithEntryIngredientInReceipt);
                            }
                        }

                        context.Configuration.ValidateOnSaveEnabled = false;
                        context.Ingredients.Attach(ingredient);
                        context.Entry(ingredient).State = EntityState.Deleted;
                        context.ChangeTracker.DetectChanges();
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception)
                    {
                        transaction.Rollback();
                        throw new InvalidOperationException("Помилка видалення.");
                    }
                    finally
                    {
                        context.Configuration.ValidateOnSaveEnabled = true;
                    }
                }
            }
        }

        public override List<Ingredient> GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Ingredients.Load();
                return context.Ingredients.Local.OrderBy(n => n.Name).ToList();
            }
        }

        public List<IngredientPackage> GetIngredientPackageDataSource()
        {
            List<IngredientPackage> dataSource;
            using (var dbcontext = new StorageDbContext())
            {
                dataSource = dbcontext.Packages.
                Join(
                dbcontext.Ingredients,
                package => package.IngredientId,
                ingredient => ingredient.Id,
                (package, ingredient) => new IngredientPackage()
                {
                    Name = ingredient.Name,
                    Weight = package.Weight
                }).OrderBy(n => n.Name).ToList();
            }
            return dataSource;
        }


    }
}
