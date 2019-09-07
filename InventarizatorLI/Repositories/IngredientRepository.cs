using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories.TableJoin;

namespace InventarizatorLI.Repositories
{
    public class IngredientRepository : IIngredientRepository
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
                else throw new ArgumentException("This ingredient already exist.", nameof(newIngredient));
            }
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Ingredient GetById(int index)
        {
            throw new NotImplementedException();
        }

        public BindingList<Ingredient> GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Ingredients.Load();
                return context.Ingredients.Local.ToBindingList();
            }
        }

        public BindingList<IngredientPackage> GetIngredientPackageDataSource()
        {
            BindingList<IngredientPackage> dataSource;
            using (var dbcontext = new StorageDbContext())
            {
                var ingredientPackages = dbcontext.Packages.
                Join(
                dbcontext.Ingredients,
                package => package.IngredientId,
                ingredient => ingredient.Id,
                (package, ingredient) => new IngredientPackage()
                {
                    Name = ingredient.Name,
                    Weight = package.Weight
                }).ToList();
                dataSource = new BindingList<IngredientPackage>(ingredientPackages);
            }
            return dataSource;
        }

        public void Remove(int index, int amount)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
