using Newtonsoft.Json.Schema;
using Omu.ValueInjecter;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.Query;
using PoolGuy.Mobile.Data.SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PoolGuy.Mobile.Data.Controllers
{
    public class CustomerController : BaseController<CustomerModel>
    {
        public CustomerController()
            :base()
        {
            
        }

        public async Task<List<CustomerModel>> SearchCustomer(string criteria)
        {
            try
            {
                var customers =  await SQLiteControllerBase
                    .DatabaseAsync
                    .QueryAsync<CustomerModel>("SELECT c.* FROM CustomerModel c " +
                                                  "LEFT OUTER JOIN AddressModel am on c.AddressId = am.Id " +
                                                  "LEFT OUTER JOIN ContactModel cm on c.ContactId = cm.Id " +
                                                  "LEFT OUTER JOIN PoolModel pm    on c.PoolId = pm.Id " +
                                                  "WHERE c.FirstName like '%" + criteria + "%' " +
                                                  "OR c.LastName like '%" + criteria + "%' " +
                                                  "OR am.Address1 like '%" + criteria + "' " +
                                                  "OR am.City like '" + criteria + "%' " +
                                                  "OR am.State like '%" + criteria + "%' " +
                                                  "OR am.Zip like '%" + criteria + "%' " +
                                                  "OR cm.Phone like '%" + criteria + "%' " +
                                                  "OR cm.CellPhone like '%" + criteria + "%' " +
                                                  "OR cm.Email like '%" + criteria + "%' " +
                                                  "ORDER BY C.FirstName").ConfigureAwait(false);

                if (!customers.Any())
                {
                    customers = await SQLiteControllerBase
                                        .DatabaseAsync
                                        .QueryAsync<CustomerModel>("SELECT c.* FROM CustomerModel c " +
                                                                     "LEFT OUTER JOIN AddressModel am on c.AddressId = am.Id " +
                                                                      "LEFT OUTER JOIN ContactModel cm on c.ContactId = cm.Id " +
                                                                      "LEFT OUTER JOIN PoolModel pm    on c.PoolId = pm.Id " +
                                                                      "ORDER BY C.FirstName " +
                                                                      "LIMIT 10").ConfigureAwait(false);

                }

                foreach (var customer in customers)
                {
                    await SQLiteControllerBase
                    .DatabaseAsync
                    .GetChildrenAsync(customer, true)
                    .ConfigureAwait(false);
                }

                return customers;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<CustomerModel>> ListWithChildrenAsync(SQLControllerListCriteriaModel criteria)
        {
            try
            {
                List<CustomerModel> list = await LocalData.List(criteria).ConfigureAwait(false);

                foreach (var model in list)
                {
                    SQLiteNetExtensions
                   .Extensions
                   .ReadOperations
                   .GetWithChildren<List<CustomerModel>>(SQLiteControllerBase.DatabaseAsync.GetConnection(), model, true);
                }
                
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task ModifyWithChildrenAsync(CustomerModel model)
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
                    model.Address.Id = Guid.NewGuid();
                    model.AddressId = model.Address.Id;
                    model.Contact.Id = Guid.NewGuid();
                    model.ContactId = model.Contact.Id;
                    model.Pool.Id = Guid.NewGuid();
                    model.PoolId = model.Pool.Id;
                    model.Created = created;
                    model.Pool.Created = created;
                    model.Address.Created = created;
                    model.Contact.Created = created;
                }
                else 
                {
                    var modified = DateTime.Now.ToUniversalTime();
                    var tempModel = (CustomerModel)new CustomerModel().InjectFrom(model);
                    
                    model = await LoadAsync(model.Id);
                    model.InjectFrom(tempModel);

                    model.Modified = modified;
                    model.Pool.Modified = model.Pool.WasModified ? modified : model.Pool.Modified;
                    model.Address.Modified = model.Address.WasModified ? modified : model.Address.Modified;
                    model.Contact.Modified = model.Contact.WasModified ? modified : model.Contact.Modified;
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

        public async Task<CustomerModel> ModifyAsync(CustomerModel model)
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
                    var tempModel = (CustomerModel)new CustomerModel().InjectFrom(model);
                    model = await LoadAsync(model.Id).ConfigureAwait(false);
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

        public async Task<bool> DeleteAsync(CustomerModel model)
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

        public async Task<CustomerModel> LoadAsync(Guid id)
        {
            try
            {
                if (id.Equals(Guid.Empty))
                {
                    return null;
                }

                // load customer
                var model = await LocalData.Load(id).ConfigureAwait(false);

                // load foreing key fields
                if (model != null)
                {
                    await SQLiteControllerBase
                        .DatabaseAsync
                        .GetChildrenAsync<CustomerModel>(model, true)
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