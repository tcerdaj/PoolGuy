using Omu.ValueInjecter;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.Query;
using PoolGuy.Mobile.Data.SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoolGuy.Mobile.Data.Controllers
{
    public class StopController : BaseController<StopModel>
    {
        public StopController(): base(){}

        public async Task<List<StopModel>> ListWithChildrenAsync(SQLControllerListCriteriaModel criteria)
        {
            try
            {
                List<StopModel> list = await LocalData.List(criteria).ConfigureAwait(false);

                foreach (var model in list)
                {
                    SQLiteNetExtensions
                   .Extensions
                   .ReadOperations
                   .GetWithChildren<List<StopModel>>(SQLiteControllerBase.DatabaseAsync.GetConnection(), model, true);
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task ModifyWithChildrenAsync(StopModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new Exception("Model can't bee null");
                }

                if (model.Customer == null)
                {
                    throw new Exception("Customer can't bee null");
                }

                if (model.Items == null || (model.Items != null && !model.Items.Any()))
                {
                    throw new Exception("Items can't bee null");
                }

                if (model.Id == Guid.Empty)
                {
                    var created = DateTime.Now.ToUniversalTime();
                    model.Id = Guid.NewGuid();
                    model.Customer.DateLastVisit = created;
                    model.Customer.Modified = created;
                }
                else
                {
                    var modified = DateTime.Now.ToUniversalTime();
                    var tempModel = (StopModel)new StopModel().InjectFrom(model);

                    model = await LoadAsync(model.Id);
                    model.InjectFrom(tempModel);

                    model.Modified = modified;
                    model.Customer.DateLastVisit = modified;
                    model.Customer.Modified = modified;
                }

                SQLiteNetExtensions
                    .Extensions
                    .WriteOperations
                    .InsertOrReplaceWithChildren(SQLiteControllerBase.DatabaseAsync.GetConnection(), model, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(StopModel model)
        {
            try
            {
                if (model == null)
                {
                    return false;
                }

                var item = await LoadAsync(model.Id).ConfigureAwait(false);

                SQLiteNetExtensions
                    .Extensions
                    .WriteOperations
                    .Delete(SQLiteControllerBase.DatabaseAsync.GetConnection(), item, true);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<StopModel> LoadAsync(Guid id)
        {
            try
            {
                if (id.Equals(Guid.Empty))
                {
                    return null;
                }

                // load customer
                var model = await LocalData.Load(id).ConfigureAwait(false);
                var m = await LocalData.List(new SQLControllerListCriteriaModel
                {
                  Filter = new List<SQLControllerListFilterField>
                  {
                      new SQLControllerListFilterField{
                          FieldName = "Id",
                          ValueLBound = id.ToString()
                      }
                  }
                });

                // load foreing key fields
                if (model != null)
                {
                    await SQLiteControllerBase
                        .DatabaseAsync
                        .GetChildrenAsync<StopModel>(model, true)
                        .ConfigureAwait(false);
                }

                return model;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}