using InventarizatorLI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarizatorLI
{
    public class SampleInitializer
        : DropCreateDatabaseIfModelChanges<StorageDbContext>
    {
        protected override void Seed(StorageDbContext context)
        {
            context.Database.ExecuteSqlCommand
                 ("CREATE LOGIN NewAdminName WITH PASSWORD = 'ABCD'");
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
