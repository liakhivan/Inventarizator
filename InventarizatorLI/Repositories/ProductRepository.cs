using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventarizatorLI.Model;
using InventarizatorLI;
using System.ComponentModel;
using InventarizatorLI.Repositories.TableJoin;
using System.Data.Entity;

namespace InventarizatorLI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public void Create(Product newProduct)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
               var product = context.Products.Where(element => element.Name == newProduct.Name);
                if (product.Count() == 0)
                {
                    context.Products.Add(newProduct);
                    context.SaveChanges();
                }
                else throw new ArgumentException("This product already exist.", nameof(newProduct));
            }
        }

        public Product GetById(int index)
        {
            throw new NotImplementedException();
        }

        public BindingList<Product> GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Products.Load();
                return context.Products.Local.ToBindingList();
            }
        }

        public void Remove(int index, int amount)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public BindingList<ProductConteiner> GetProductConteinerDataSource()
        {

            BindingList<ProductConteiner> dataSource;
            using (var dbcontext = new StorageDbContext())
            {
                var productConteiners = dbcontext.Conteiners.
                Join(
                dbcontext.Products,
                conteiner => conteiner.ProductId,
                product => product.Id,
                (conteiner, product) => new ProductConteiner()
                {
                    Name = product.Name,
                    Weight = conteiner.Weight,
                    Amount = conteiner.Amount
                }).ToList();
                dataSource = new BindingList<ProductConteiner>(productConteiners);
            }
            return dataSource;
        }

    }
}
