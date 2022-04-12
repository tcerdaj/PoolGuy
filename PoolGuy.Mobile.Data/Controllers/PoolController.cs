using Omu.ValueInjecter;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PoolGuy.Mobile.Data.Controllers
{
    public class PoolController : BaseController<PoolModel>
    {
        public PoolController()
            : base()
        {

        }

        public async Task<PoolModel> LoadAsync(Guid id)
        {
            try
            {
                if (id.Equals(Guid.Empty))
                {
                    return null;
                }

                // load foreing key fields
               var model =  await SQLiteControllerBase
                    .DatabaseAsync
                    .FindWithChildrenAsync<PoolModel>(id, true);

                if (model != null)
                {
                    model.Images = new System.Collections.ObjectModel.ObservableCollection<EntityImageModel>( await new ImageController().LocalData.List(new Models.Query.SQLControllerListCriteriaModel
                    {
                        Filter = new System.Collections.Generic.List<Models.Query.SQLControllerListFilterField> {
                              new Models.Query.SQLControllerListFilterField 
                              {
                                  FieldName = "EntityId",
                                  ValueLBound = model.Id.ToString()
                              } 
                        }
                    }));
                }

                return model;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ModifyWithChildrenAsync(PoolModel model)
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
                    model.Equipments.LastOrDefault().Id = Guid.NewGuid();
                    model.Equipments.LastOrDefault().Created = created;
                    model.Equipments.LastOrDefault().PoolId = model.Id;

                    // Add pool images
                    if (model.Images != null && model.Images.Any())
                    {
                        await new ImageController().LocalData.InsertAll(model.Images.Select(x => new EntityImageModel
                        {
                            EntityId = model.Id,
                            ImageUrl = x.ImageUrl,
                            ImageType = Enums.ImageType.Pool

                        }).ToList());
                    }
                }
                else
                {
                    var tempModel = (PoolModel)new PoolModel().InjectFrom(model);
                    var poolModel = await LoadAsync(model.Id);
                    
                    if (poolModel != null)
                    {
                        model = poolModel;
                    }

                    // Delete current images
                    await new ImageController().DeleteAllImagesAsync(model.Id, Enums.ImageType.Pool);

                    // Add new images
                    if (model.Images != null && model.Images.Any())
                    {
                        await new ImageController().LocalData.InsertAll(model.Images.Select(x => new EntityImageModel
                        {
                            EntityId = model.Id,
                            ImageUrl = x.ImageUrl,
                            ImageType = Enums.ImageType.Pool

                        }).ToList());
                    }
                    
                    model.InjectFrom(tempModel);
                    model.Modified = DateTime.Now.ToUniversalTime();
                }

                SQLiteNetExtensions.Extensions.WriteOperations.InsertOrReplaceWithChildren(SQLiteControllerBase.DatabaseAsync.GetConnection(), model, true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }    
}