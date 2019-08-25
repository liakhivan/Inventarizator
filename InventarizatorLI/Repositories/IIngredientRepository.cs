using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using InventarizatorLI.Model;
using InventarizatorLI.Repositories.TableJoin;

namespace InventarizatorLI.Repositories
{
    interface IIngredientRepository : IGenericRepository<Ingredient, int>
    {
    }
}
