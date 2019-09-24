using System.Collections.Generic;
using System.ComponentModel;

namespace InventarizatorLI.Repositories
{
    public interface IGenericRepository<T, ID> where T: class
    {
        T GetById(int index);
        List<T> GetDataSource();
    }
}
