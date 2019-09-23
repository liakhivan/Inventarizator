using System.Collections.Generic;
using System.ComponentModel;

namespace InventarizatorLI.Repositories
{
    public interface IGenericRepository<T, ID> where T: class
    {
        void Update();
        T GetById(int index);
        List<T> GetDataSource();
    }
}
