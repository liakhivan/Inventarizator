using System;

namespace InventarizatorLI.Repositories
{
    public interface IGenericRepository<T, ID> where T: class
    {
        void Create(T element);
        void Update();
        void Remove(int index, int amount);
        T GetById(int index);
    }
}
