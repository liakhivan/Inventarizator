using System;
using System.Collections.Generic;
using System.IO;
using InventarizatorLI.Model;

namespace InventarizatorLI.Repositories
{
    public abstract class GenericRepository<T> where T: class
    {
        public abstract List<T> GetDataSource();
        public void BackupData(string patch)
        {
            using (var context = new StorageDbContext())
            {
                patch = patch + "Backup-" + DateTime.Today.ToShortDateString() + ".bak";
                if (!File.Exists(patch))
                {
                    var file = File.Create(patch);
                    file.Close();
                }
                context.Database.ExecuteSqlCommand(
                    System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction,
                    "BACKUP DATABASE " + context.Database.Connection.Database + " TO DISK = \'" + patch + "\' WITH INIT;");
            }
        }

        public void RecoveryData(string patch)
        {

        }
    }
}
