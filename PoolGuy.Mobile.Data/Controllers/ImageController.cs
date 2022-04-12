using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.Query;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolGuy.Mobile.Data.Controllers
{
    public class ImageController: BaseController<EntityImageModel>
    {
        public ImageController()
            :base()
        {

        }

        public async Task<bool> DeleteAllImagesAsync(Guid entityId, Enums.ImageType type)
        {
            if (entityId == Guid.Empty)
            {
                return false;
            }

            try
            {
                var criteria = new SQLControllerListCriteriaModel
                {
                    Filter = new List<SQLControllerListFilterField>
                  {
                      new SQLControllerListFilterField{
                          FieldName = "EntityId",
                          ValueLBound = entityId.ToString()
                      },
                      new SQLControllerListFilterField{
                          FieldName = "ImageType",
                          ValueLBound = ((int)type).ToString()
                      }
                  }
                };

                var images = await LocalData.List(criteria);
                foreach (var image in images)
                {
                    await LocalData.Delete(image);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }
    }
}