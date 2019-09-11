using System.Collections.Generic;
using InventarizatorLI.Model;

namespace InventarizatorLI.Comparators
{
    class PackageEqualityComparer : IEqualityComparer<Package>
    {
        public bool Equals(Package x, Package y)
        {
            if(x.Ingredient == y.Ingredient & x.IngredientId == y.IngredientId)
                return true;
            else 
                return false;
        }

        public int GetHashCode(Package obj)
        {
            return ((obj.Id * obj.IngredientId));
        }
    }
}
