using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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
                patch = patch + "database" + "-" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".bak";
                if (!File.Exists(patch))
                {
                    var file = File.Create(patch);
                    file.Close();
                }
                context.Database.ExecuteSqlCommand(
                    TransactionalBehavior.DoNotEnsureTransaction,
                    "BACKUP DATABASE " + context.Database.Connection.Database + " TO DISK = \'" + patch + "\' WITH INIT;");
            }
        }

        public void RestoreData(string patch)
        {
            using (var context = new StorageDbContext())
            {
                if (!patch.Contains(".bak"))
                    throw new ArgumentException();

                string database = context.Database.Connection.Database;

                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                "ALTER DATABASE " + database + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE");

                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                "USE MASTER RESTORE DATABASE " + database + " FROM DISK=\'" + patch + "\' WITH REPLACE;");

                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                "ALTER DATABASE " + database + " SET MULTI_USER");
            }
        }
    }
}
