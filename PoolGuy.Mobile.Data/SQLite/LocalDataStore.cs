using SQLite;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.Query;
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
                        await DatabaseAsync.CreateTableAsync(typeof(T), CreateFlags.None).ConfigureAwait(false);
                        
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
                        model.Created = DateTime.Now.ToUniversalTime();
                    }

                    await DatabaseAsync.InsertAsync(model).ConfigureAwait(false);
                    return model;
                }
                else
                {
                    model.Modified = DateTime.Now.ToUniversalTime();
                    await DatabaseAsync.UpdateAsync(model).ConfigureAwait(false);
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
                    await DatabaseAsync.DeleteAsync(model).ConfigureAwait(false);
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
                foreach (var model in list)
                {
                    if (model.Id == Guid.Empty)
                    {
                        model.Id = Guid.NewGuid();

                        if (model.Created == null || model.Created == DateTime.MinValue)
                        {
                            model.Created = DateTime.Now.ToUniversalTime();
                        }
                    }
                    else
                    {
                        model.Modified = DateTime.Now.ToUniversalTime();
                    }
                }

                await DatabaseAsync.InsertAllAsync(list).ConfigureAwait(false);
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
                await CreateTabletIfNotExist();

                return await DatabaseAsync.Table<T>().ToListAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception at List: {0} {1}", typeof(T).Name, e);
                throw;
            }
        }

        private async Task CreateTabletIfNotExist()
        {
            if (DatabaseAsync.TableMappings.All(m => m.MappedType.Name != typeof(T).Name))
            {
                await DatabaseAsync.CreateTableAsync(typeof(T), CreateFlags.None);

                AddRegisteredTablet(typeof(T).Name);
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

                await CreateTabletIfNotExist();

                criteria.View = typeof(T).Name;

                var query = SQLQuery.BuildListQuery<T>(criteria);

                return await DatabaseAsync.QueryAsync<T>(query).ConfigureAwait(false);
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
                
                await CreateTabletIfNotExist();

                return await DatabaseAsync.Table<T>().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
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
                    await DatabaseAsync.DropTableAsync<T>().ConfigureAwait(false);
                    await DatabaseAsync.CreateTableAsync<T>().ConfigureAwait(false);
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
