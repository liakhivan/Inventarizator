using System;
using System.Collections.Generic;
using System.Data.Entity;
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

                string databasePath = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    databasePath = Directory.GetParent(databasePath).ToString();
                }

                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    "ALTER DATABASE " + database + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE");

                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    "USE MASTER RESTORE DATABASE " + database + " FROM DISK=\'" + patch + "\' WITH REPLACE, " +
                    "MOVE \'StorageDBConnection\' to \'" + databasePath + "\\StorageDBConnection.mdf\', " +
                    "MOVE \'StorageDBConnection_log\' to \'" + databasePath + "\\StorageDBConnection.ldf\'");

                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    "ALTER DATABASE " + database + " SET MULTI_USER");
            }
        }

        public static void FormatingAllData()
        {
            using (var context = new StorageDbContext())
            {
                throw new Exception();

                //context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                //    "ALTER DATABASE " + database + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE");

                //context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                //    "USE MASTER RESTORE DATABASE " + database + " FROM DISK=\'" + patch + "\' WITH REPLACE, " +
                //    "MOVE \'StorageDBConnection\' to \'" + databasePath + "\\StorageDBConnection.mdf\', " +
                //    "MOVE \'StorageDBConnection_log\' to \'" + databasePath + "\\StorageDBConnection.ldf\'");

                //context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                //    "ALTER DATABASE " + database + " SET MULTI_USER");
            }
        }
    }
}
