using System;
using System.ComponentModel;

namespace InventarizatorLI.Repositories
{
    interface IGenericRepository<T, ID> where T: class
    {
        void Create(T newElement);
        void Update();
        void Remove(int index, int amount);
        T GetById(int index);
        BindingList<T> GetDataSource();
    }
}
