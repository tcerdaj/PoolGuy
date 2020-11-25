using SQLite;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.Query;
using PoolGuy.Mobile.Data.Extentions;
using PoolGuy.Mobile.Data.Helpers;
using System.Diagnostics;

namespace PoolGuy.Mobile.Data.SQLite
{
    public class LocalDataStore<T> : SQLiteControllerBase, ILocalDataStore<T> where T : EntityBase, new()
    {
        private static bool Initialized 
        {
            get
            {
                return Settings.TabletsRegistered.Any(x => x.Equals(typeof(T).Name));
            }
        }

        public LocalDataStore()
        {
        }

        public async Task CreateTableAsync()
        {
            try
            {
                if (!Initialized)
                {
                    if (DatabaseAsync.TableMappings.All(m => m.MappedType.Name != typeof(T).Name))
                    {
                        await DatabaseAsync.CreateTableAsync(typeof(T), CreateFlags.None);
                        
                        AddRegisteredTablet(typeof(T).Name);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception at ClearTableAsync: {0} {1}", typeof(T).Name, e);
                throw;
            }
        }

        void AddRegisteredTablet(string tabletName)
        {
            try
            {
                var tablets = Settings.TabletsRegistered.ToList();

                if (!tablets.Any(x => x.Equals(tabletName)))
                {
                    tablets.Add(tabletName);
                    Settings.TabletsRegistered = tablets.ToArray();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task<T> Modify(T model)
        {
            try
            {
                if (model == null)
                {
                    return null;
                }

                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();

                    if (model.Created == null || model.Created == DateTime.MinValue)
                    {
                        model.Created = DateTime.Now;
                    }

                    await DatabaseAsync.InsertAsync(model);
                    return model;
                }
                else
                {
                    model.Modified = DateTime.Now;
                    await DatabaseAsync.UpdateAsync(model);
                    return (T)model;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception at Modify: {0} {1}", typeof(T).Name, e);
                throw e;
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    await Delete(await Load(id));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception at Delete Id: {0} {1}", typeof(T).Name, e);
                throw e;
            }
        }

        public async Task Delete(T model)
        {
            try
            {
                if (model != null && model.Id != Guid.Empty)
                {
                    await DatabaseAsync.DeleteAsync(model);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception at Delete: {0} {1}", typeof(T).Name, e);
                throw e;
            }
        }

        public async Task InsertAll(List<T> list)
        {
            try
            {
                await DatabaseAsync.InsertAllAsync(list);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception at InsertAll: {0} {1}", typeof(T).Name, e);
                throw;
            }
        }

        public async Task<List<T>> List()
        {
            try
            {
                return await DatabaseAsync.Table<T>().ToListAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception at List: {0} {1}", typeof(T).Name, e);
                throw;
            }
        }

        public async Task<List<T>> List(SQLControllerListCriteriaModel criteria)
        {
            try
            {
                if (criteria == null)
                {
                    return null;
                }

                criteria.View = typeof(T).Name;

                var query = SQLQuery.BuildListQuery<T>(criteria);

                return await DatabaseAsync.QueryAsync<T>(query);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception at List criteria: {0} {1}", typeof(T).Name, e);
                throw;
            }
        }

        public async Task<T> Load(Guid id)
        {
            try
            {
                if (!Initialized)
                    return null;

                return await DatabaseAsync.Table<T>().FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception at Load Id: {0} {1}", typeof(T).Name, e);
                throw;
            }
        }

        public async Task ClearTableAsync()
        {
            try
            {
                if (Initialized)
                {
                    await DatabaseAsync.DropTableAsync<T>();
                    await DatabaseAsync.CreateTableAsync<T>();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception at ClearTableAsync: {0} {1}", typeof(T).Name, e);
                throw;
            }
        }
    }
}
