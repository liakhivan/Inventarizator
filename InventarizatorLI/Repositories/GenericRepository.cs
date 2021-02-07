using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Threading;
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

        public void RestoreData(object patch)
        {
            using (var context = new StorageDbContext())
            {
                if (!patch.ToString().Contains(".bak"))
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
                    "USE MASTER RESTORE DATABASE " + database + " FROM DISK=\'" + patch.ToString() + "\' WITH REPLACE, " +
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
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "TRUNCATE TABLE ProductStatistic");
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "TRUNCATE TABLE IngredientStatistic");
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "TRUNCATE TABLE IngredientsForProducts");
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "TRUNCATE TABLE Conteiners");
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "TRUNCATE TABLE Packages");
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "TRUNCATE TABLE Tares");
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "TRUNCATE TABLE Clients");
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "TRUNCATE TABLE Securities");
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, @"DELETE FROM Ingredients WHERE ID != -1");
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, @"DELETE FROM Products WHERE ID != -1");
            }
        }
    }
}
