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

                // load customer
                var model = await LocalData.Load(id);

                // load foreing key fields
                await SQLiteControllerBase.DatabaseAsync.GetChildrenAsync<PoolModel>(model, true);

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
                    var created = DateTime.Now;
                    model.Id = Guid.NewGuid();
                    model.Equipments.LastOrDefault().Id = Guid.NewGuid();
                    model.Equipments.LastOrDefault().Created = created;
                    model.Equipments.LastOrDefault().PoolId = model.Id;
                    model.Equipments.LastOrDefault().CustomerId = model.CustomerId;
                }

                await SQLiteControllerBase
                     .DatabaseAsync
                     .InsertOrReplaceWithChildrenAsync(model, true)
                     .ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }    
}