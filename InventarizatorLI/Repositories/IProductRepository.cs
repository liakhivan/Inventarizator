using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventarizatorLI.Model;
using System.ComponentModel;
using InventarizatorLI.Repositories.TableJoin;

namespace InventarizatorLI.Repositories
{
    interface IProductRepository: IGenericRepository<Product, int>
    {
    }
}
