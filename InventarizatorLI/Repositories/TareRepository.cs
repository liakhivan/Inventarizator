using InventarizatorLI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarizatorLI.Repositories
{
    public class TareRepository : GenericRepository<Tare>
    {
        public void Add(Tare tare)
        {
            using (StorageDbContext dbContext = new StorageDbContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (dbContext.Tares.FirstOrDefault(n => n.Name == tare.Name) == null)
                        {
                            dbContext.Tares.Add(tare);
                        }
                        else
                        {
                            dbContext.Tares.First(n => n.Name == tare.Name).Amount += tare.Amount;
                        }

                        dbContext.SaveChanges();
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

        public void Edit(Tare newTare)
        {
            using (StorageDbContext dbContext = new StorageDbContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (newTare.Name == null || newTare.Name == "")
                            throw new Exception("Не вказано назву тари.");
                        dbContext.Clients.First(n => n.Id == newTare.Id).Name = newTare.Name;

                        dbContext.SaveChanges();
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

        public void Remove(Tare tareForRemoving)
        {
            using (StorageDbContext dbContext = new StorageDbContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (tareForRemoving.Name == null || tareForRemoving.Name == "")
                            throw new Exception("Не вказано назву тари.");
                        if(tareForRemoving.Amount <= 0)
                            throw new Exception("Не вказано кількість тари.");
                        dbContext.Tares.First(n => n.Name == tareForRemoving.Name).Amount -= tareForRemoving.Amount;

                        dbContext.SaveChanges();
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

        public void DeleteById(int id)
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Configuration.ValidateOnSaveEnabled = false;
                        context.Configuration.AutoDetectChangesEnabled = false;
                        var tareForDeleting = context.Tares.First(element => element.Id == id);

                        context.Tares.Attach(tareForDeleting);
                        context.Entry(tareForDeleting).State = EntityState.Deleted;

                        context.ChangeTracker.DetectChanges();
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        context.Configuration.ValidateOnSaveEnabled = true;
                    }
                }
            }
        }

        public override List<Tare> GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Tares.Load();
                return context.Tares.Local.ToList();
            }
        }
    }
}
