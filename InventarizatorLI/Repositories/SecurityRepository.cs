using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories;

namespace InventarizatorLI.Repositories
{
    public class SecurityRepository : GenericRepository<Security>
    {

        public void Edit(string newPassStr)
        {
            using (StorageDbContext dbContext = new StorageDbContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (newPassStr.Length < 4)
                            throw new Exception("Пароль занадто короткий. Він має бути не менше 4 символів.");

                        dbContext.Securities.FirstOrDefault().Pass = newPassStr.GetHashCode();

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

        public override List<Security> GetDataSource()
        {
            throw new NotImplementedException();
        }
    }
}
