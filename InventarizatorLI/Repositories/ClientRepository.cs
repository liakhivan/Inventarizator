using InventarizatorLI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarizatorLI.Repositories
{
    public class ClientRepository : GenericRepository<Client>
    {
        public void Create(Client client)
        {
            using (StorageDbContext dbContext = new StorageDbContext())
            {
                using(var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Client newEntryClient = dbContext.Clients.FirstOrDefault(n => n.Name == client.Name);

                        if (newEntryClient != null)
                            throw new Exception("Даний клієнт вже існує.");
                        dbContext.Clients.Add(client);

                        dbContext.SaveChanges();
                        transaction.Commit();

                    } catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public void Edit(Client newClient)
        {
            using (StorageDbContext dbContext = new StorageDbContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (newClient.Name == null || newClient.Name == "")
                            throw new Exception("Не вказано ім'я клієнта.");
                        dbContext.Clients.First(n => n.Id == newClient.Id).Name = newClient.Name;

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
                        var clientForDeleting = context.Clients.First(element => element.Id == id);

                        context.Clients.Attach(clientForDeleting);
                        context.Entry(clientForDeleting).State = EntityState.Deleted;

                        context.ChangeTracker.DetectChanges();
                        context.SaveChanges();

                        transaction.Commit();
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

        public override List<Client> GetDataSource()
        {
            using (StorageDbContext context = new StorageDbContext())
            {
                context.Clients.Load();
                return context.Clients.Local.ToList();
            }
        }
    }
}
