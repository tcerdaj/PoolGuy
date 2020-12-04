using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.Query;
using PoolGuy.Mobile.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using SQLiteNetExtensionsAsync.Extensions;

namespace PoolGuy.Mobile.Data.Controllers
{
    public class SchedulerController : BaseController<SchedulerModel>
    {
        public SchedulerController()
            : base()
        {

        }

        public async Task<List<SchedulerModel>> ListWithChildrenAsync(SQLControllerListCriteriaModel criteria)
        {
            try
            {
                List<SchedulerModel> list = await LocalData.List(criteria);

                foreach (var model in list)
                {
                    SQLiteNetExtensions
                   .Extensions
                   .ReadOperations
                   .GetWithChildren<List<SchedulerModel>>(SQLiteControllerBase.DatabaseAsync.GetConnection(), model, true);
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ModifyWithChildrenAsync(SchedulerModel model)
        {
            try
            {
                if (model == null)
                {
                    return;
                }

                if (model.Id == Guid.Empty)
                {
                    var created = DateTime.Now.ToUniversalTime();
                    model.Id = Guid.NewGuid();
                    model.Created = created;
                }
                else
                {
                    var modified = DateTime.Now.ToUniversalTime();
                    var tempModel = (SchedulerModel)new SchedulerModel().InjectFrom(model);

                    model = await LoadAsync(model.Id);
                    model.InjectFrom(tempModel);
                    model.Modified = modified;
                }

                SQLiteNetExtensions.Extensions.WriteOperations.InsertOrReplaceWithChildren(SQLiteControllerBase.DatabaseAsync.GetConnection(), model, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SchedulerModel> ModifyAsync(SchedulerModel model)
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
                    model.Created = DateTime.Now.ToUniversalTime();
                }
                else
                {
                    var tempModel = (SchedulerModel)new SchedulerModel().InjectFrom(model);
                    model = await LoadAsync(model.Id);
                    model.InjectFrom(tempModel);
                    model.Modified = DateTime.Now.ToUniversalTime();
                }

                return await LocalData
                    .Modify(model)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteAsync(SchedulerModel model)
        {
            try
            {
                if (model == null)
                {
                    return false;
                }

                var item = await LoadAsync(model.Id);
                SQLiteNetExtensions.Extensions.WriteOperations.Delete(SQLiteControllerBase.DatabaseAsync.GetConnection(), item, true);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<SchedulerModel> LoadAsync(Guid id)
        {
            try
            {
                if (id.Equals(Guid.Empty))
                {
                    return null;
                }

                // load customer
                var model = await LocalData.Load(id);

                // load foreing key fields
                await SQLiteControllerBase.DatabaseAsync.GetChildrenAsync<SchedulerModel>(model, true);

                return model;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}