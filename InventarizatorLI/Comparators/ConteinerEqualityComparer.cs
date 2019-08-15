using System;
using System.Collections.Generic;
using InventarizatorLI.Model;

namespace InventarizatorLI.Comparators
{
    public partial class ConteinerEqualityComparer : IEqualityComparer<Conteiner>
    {
        public bool Equals(Conteiner x, Conteiner y)
        {
            if ((x.Weight == y.Weight) & (x.Product.Id == y.Product.Id))
            {
                return true;
            }
            else
                return false;
        }

        public int GetHashCode(Conteiner obj)
        {
            return obj.Weight.GetHashCode();
        }
    }
}
