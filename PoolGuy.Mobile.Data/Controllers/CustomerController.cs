using Newtonsoft.Json;
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
                                                  "LEFT OUTER JOIN AddressModel am on c.Id = am.CustomerId " +
                                                  "LEFT OUTER JOIN ContactModel cm on c.Id = cm.CustomerId " +
                                                  "LEFT OUTER JOIN PoolModel pm    on c.Id = pm.CustomerId " +
                                                  "WHERE c.FirstName like '%" + criteria + "%' " +
                                                  "OR c.LastName like '%" + criteria + "%' " +
                                                  "OR am.Address1 like '%" + criteria + "' " +
                                                  "OR am.City like '" + criteria + "%' " +
                                                  "OR am.State like '%" + criteria + "%' " +
                                                  "OR am.Zip like '%" + criteria + "%' " +
                                                  "OR cm.Phone like '%" + criteria + "%' " +
                                                  "OR cm.CellPhone like '%" + criteria + "%' " +
                                                  "OR cm.Email like '%" + criteria + "%' " +
                                                  "ORDER BY C.FirstName");
                
                foreach (var customer in customers)
                {
                    await SQLiteControllerBase
                    .DatabaseAsync
                    .GetChildrenAsync<CustomerModel>(customer, true);
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
                List<CustomerModel> customers = await LocalData.List(criteria);

                foreach (var customer in customers)
                {
                    await SQLiteControllerBase
                    .DatabaseAsync
                    .GetChildrenAsync<CustomerModel>(customer, true);
                }
                
                return customers;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task ModifyWithChildrenAsync(CustomerModel customer)
        {
            try
            {
                await SQLiteControllerBase
                     .DatabaseAsync
                     .InsertOrReplaceWithChildrenAsync(customer, true)
                     .ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PoolModel> ModifyPoolAsync(PoolModel model)
        {
            try
            {
                if (model == null)
                {
                    return null;
                }
                
                var controller = new PoolController();

                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                    model.Created = DateTime.Now;
                }
                else
                {
                    model.Modified = DateTime.Now;
                }

                return await controller.LocalData.Modify(model);
            }
            catch (System.Exception e)
            {
                throw e;
            }

        }

        public async Task<bool> DeletePoolAsync(PoolModel model)
        {
            try
            {
                if (model == null)
                {
                    return false;
                }

                var controller = new PoolController();
                await controller.LocalData.Delete(model);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<AddressModel> ModifyAddressAsync(AddressModel model)
        {
            try
            {
                if (model == null)
                {
                    return null;
                }

                var controller = new AddressController();

                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                    model.Created = DateTime.Now;
                }
                else
                {
                    model.Modified = DateTime.Now;
                }

                return await controller.LocalData.Modify(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteAddressAsync(AddressModel model)
        {
            try
            {
                if (model == null)
                {
                    return false;
                }

                var controller = new AddressController();
                await controller.LocalData.Delete(model);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ContactModel> ModifyContactAsync(ContactModel model)
        {
            try
            {
                if (model == null)
                {
                    return null;
                }

                var controller = new ContactInformationController();

                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                    model.Created = DateTime.Now;
                }
                else
                {
                    model.Modified = DateTime.Now;
                }

                return await controller.LocalData.Modify(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteContactInformationAsync(ContactModel model)
        {
            try
            {
                if (model == null)
                {
                    return false;
                }

                var controller = new ContactInformationController();
                await controller.LocalData.Delete(model);

                return true;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<CustomerModel> ModifyAsync(CustomerModel customer)
        {
            try
            {
                if (customer == null)
                {
                    return null;
                }
                
                if (customer.Id == Guid.Empty)
                {
                    customer.Id = Guid.NewGuid();
                    customer.Created = DateTime.Now;
                }
                else
                {
                    customer.Modified = DateTime.Now;
                }

                return await LocalData.Modify(customer);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteAsync(CustomerModel customer)
        {
            try
            {
                if (customer == null)
                {
                    return false;
                }

                await SQLiteControllerBase.DatabaseAsync.DeleteAsync(customer, true);

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
                var model = await LocalData.Load(id);

                // load foreing key fields
                await SQLiteControllerBase.DatabaseAsync.GetChildrenAsync<CustomerModel>(model, true);

                return model;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}