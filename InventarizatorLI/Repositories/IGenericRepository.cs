using System.ComponentModel;

namespace InventarizatorLI.Repositories
{
    public interface IGenericRepository<T, ID> where T: class
    {
        void Update();
        void Remove(int index, int amount);
        T GetById(int index);
        BindingList<T> GetDataSource();
    }
}
