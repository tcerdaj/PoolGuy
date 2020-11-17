using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.Query;
using PoolGuy.Mobile.Data.SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SQLiteNetExtensionsAsync.Extensions;
using System.Collections.ObjectModel;

namespace PoolGuy.Mobile.Data.Controllers
{
    public class EquipmentController : BaseController<EquipmentModel>
    {
        public EquipmentController()
            :base()
        {
            
        }

        public async Task<List<EquipmentModel>> ListWithChildrenAsync(SQLControllerListCriteriaModel criteria)
        {
            try
            {
                List<EquipmentModel> list = await LocalData.List(criteria).ConfigureAwait(false);

                foreach (var model in list)
                {
                    await SQLiteControllerBase
                    .DatabaseAsync
                    .GetChildrenAsync<EquipmentModel>(model, true);
                }

                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}