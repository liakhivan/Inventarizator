using System;
using System.Collections.Generic;
using InventarizatorLI.Model;

namespace InventarizatorLI.Comparators
{
    public class ConteinerEqualityComparer : IEqualityComparer<Conteiner>
    {
        public bool Equals(Conteiner x, Conteiner y)
        {
            if(x == null || y == null)
                throw new NullReferenceException();
            if ((x.Weight == y.Weight) & (x.ProductId == y.ProductId))
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
