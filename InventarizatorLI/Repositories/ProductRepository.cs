using System;
using System.Linq;
using System.Data.Entity;
using InventarizatorLI.Model;
using System.Collections.Generic;
using InventarizatorLI.Repositories.TableJoin;

namespace InventarizatorLI.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public void CreateWithoutRecipe(Product newProduct)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var product = context.Products.Where(element => element.Name == newProduct.Name);
                        if (!product.Any())
                        {
                            context.Products.Add(newProduct);
                            context.ChangeTracker.DetectChanges();
                            context.SaveChanges();
                            transaction.Commit();
                        }
                        else throw new ArgumentException("Цей продукт вже існує.");
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new ArgumentException(e.Message);
                    }
                }
            }
        }

        public void CreateWithRecipe(Product newProduct, Dictionary<Ingredient, double> receipt)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var product = context.Products.Where(element => element.Name == newProduct.Name);
                        if (!product.Any())
                        {
                            context.Products.Add(newProduct);
                            context.ChangeTracker.DetectChanges();
                            context.SaveChanges();
                            var currentProduct = context.Products.FirstOrDefault(buffProduct => buffProduct.Name == newProduct.Name); 

                            foreach (var element in receipt)
                            {
                                context.IngredientsForProducts.Add(new IngredientsForProduct(currentProduct, element.Key, element.Value));
                            }
                            context.ChangeTracker.DetectChanges();
                            context.SaveChanges();
                            transaction.Commit();
                        }
                        else throw new ArgumentException("Цей продукт вже існує.");
                    }
                    catch(Exception e)
                    {
                        transaction.Rollback();
                        throw new ArgumentException(e.Message);
                    }
                }
            }
        }

        public void Edit(Product newProduct, Dictionary<Ingredient, double> newReceipt)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        IngredientsForProductRepository ingredientsForProductRepository = new IngredientsForProductRepository();

                        var currentProduct = context.Products.FirstOrDefault(element => element.Id == newProduct.Id);

                        if (currentProduct == null)
                        {
                            throw new ArgumentNullException("Цей продукт не існує");
                        }

                        currentProduct.Name = newProduct.Name;

                        ingredientsForProductRepository.Delete(currentProduct);

                        foreach (var element in newReceipt)
                        {
                            context.IngredientsForProducts.Add(new IngredientsForProduct(currentProduct, element.Key, element.Value));
                        }

                        context.ChangeTracker.DetectChanges();
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new ArgumentException(e.Message);
                    }
                }
            }
        }

        public void Delete(Product element)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        ConteinerRepository conteiner = new ConteinerRepository();
                        conteiner.DeleteAll(element);

                        IngredientsForProductRepository recept = new IngredientsForProductRepository();
                        recept.Delete(element);

                        context.Configuration.ValidateOnSaveEnabled = false;
                        context.Products.Attach(element);
                        context.Entry(element).State = EntityState.Deleted;

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

        public override List<Product> GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Products.Load();
                return context.Products.Local.OrderBy(n => n.Name).ToList();
            }
        }

        public List<ProductConteiner> GetProductConteinerDataSource()
        {

            List<ProductConteiner> dataSource;
            using (var dbcontext = new StorageDbContext())
            {
                dataSource = dbcontext.Conteiners.
                Join(
                dbcontext.Products,
                conteiner => conteiner.ProductId,
                product => product.Id,
                (conteiner, product) => new ProductConteiner()
                {
                    Name = product.Name,
                    Weight = conteiner.Weight,
                    Amount = conteiner.Amount
                }).OrderBy(n => n.Weight).ToList().OrderBy(n => n.Name).ToList();
            }
            return dataSource;
        }

    }
}
