using System;
using System.Collections.Generic;
using InventarizatorLI.Model;

namespace InventarizatorLI.Repositories
{
    public abstract class GenericRepository<T> where T: class
    {
        public abstract List<T> GetDataSource();
        public bool BackupData(string patch)
        {
            try
            {
                using (var context = new StorageDbContext())
                {
                    context.Database.ExecuteSqlCommand(
                        $"BACKUP DATABASE StorageDBConnection " +
                        $"TO DISK {patch}Backup.bak");
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
