using System;
using System.Threading.Tasks;
using PoolGuy.Mobile.Data.Models.Query;
using System.Collections.Generic;

namespace PoolGuy.Mobile.Data.SQLite
{
    public interface ILocalDataStore<T>
    {
        Task CreateTableAsync();
        Task ClearTableAsync();
        Task<T> Modify(T model);
        Task Delete(Guid id);
        Task Delete(T model);
        Task<T> Load(Guid id);
        Task<List<T>> List();
        Task<List<T>> List(SQLControllerListCriteriaModel criteria);
        Task InsertAll(List<T> list);
    }
}