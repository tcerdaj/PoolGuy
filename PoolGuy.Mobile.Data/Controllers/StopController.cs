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
                    model.Customer = await new CustomerController().LocalData.Load(model.CustomerId);
                    model.Items = new System.Collections.ObjectModel.ObservableCollection<StopItemModel>(await new ItemController().LocalData.List(new SQLControllerListCriteriaModel
                    {
                        Filter = new List<SQLControllerListFilterField> { new SQLControllerListFilterField { FieldName = "StopId", ValueLBound = model.Id.ToString() } }
                    }));
                    model.Images = await new ImageController().LocalData.List(new SQLControllerListCriteriaModel
                    {
                        Filter = new List<SQLControllerListFilterField> { new SQLControllerListFilterField { FieldName = "EntityId", ValueLBound = model.Id.ToString() } }
                    });
                    model.User = await new UserController().LocalData.Load(model.UserId);
                }

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);  
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

                bool newStop = false;
                if (model.Id == Guid.Empty)
                {
                    var created = DateTime.Now.ToUniversalTime();
                    model.Id = Guid.NewGuid();
                    model.Created = created;
                    model.Customer.DateLastVisit = created;
                    newStop = true;
                }
                else
                {
                    model.Modified = DateTime.Now.ToUniversalTime();
                }

                // Items
                if (model.Items != null)
                {
                    foreach (var item in model.Items)
                    {
                        if (item.Id == Guid.Empty)
                        {
                            item.Id = Guid.NewGuid();
                        }

                        item.StopId = model.Id;
                    }
                }

                // Images
                if (model.Images != null)
                {
                    foreach (var image in model.Images)
                    {
                        if (image.Id == Guid.Empty)
                        {
                            image.Id = Guid.NewGuid();
                        }

                        image.EntityId = model.Id;
                    }
                }

                SQLiteNetExtensions
                    .Extensions
                    .WriteOperations
                    .InsertOrReplaceWithChildren(SQLiteControllerBase.DatabaseAsync.GetConnection(), model, true);

                if (newStop && model.Customer != null)
                {
                    model.Modified = DateTime.Now.ToUniversalTime();
                    await new CustomerController().LocalData.Modify(model.Customer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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