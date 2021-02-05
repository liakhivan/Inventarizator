using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories;

namespace InventarizatorLI.Repositories
{
    public class SecurityRepository
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

                        dbContext.Securities.FirstOrDefault().Pass = Security.GetHashPass(newPassStr);

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

        public bool VerifyPassword(string password)
        {
            bool result = false;
            using (StorageDbContext dbContext = new StorageDbContext())
            {
                Security currPassword = dbContext.Securities.FirstOrDefault();

                if (currPassword == null)
                {
                    dbContext.Securities.Add(new Security());
                    currPassword = dbContext.Securities.FirstOrDefault();
                }

                result = currPassword.Pass == Security.GetHashPass(password);
            }

            return result;
        }
    }
}
