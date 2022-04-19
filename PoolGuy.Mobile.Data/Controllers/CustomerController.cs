using Newtonsoft.Json;
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
    public class CustomerController : BaseController<CustomerModel>
    {
        public CustomerController()
            :base()
        {
            
        }

        public async Task<List<CustomerModel>> GetCustomersBySchedulerAsync(Guid schedulerId)
        {
            try
            {

                var customers = await SQLiteControllerBase
                    .DatabaseAsync
                    .QueryAsync<CustomerModel>("SELECT " +
                                                "c.Id, " +
                                                "c.Created, " +
                                                "c.Modified, " +
                                                "c.WasModified, " +
                                                "c.HomeAddressId, " +
                                                "c.AddressId, " +
                                                "c.WasModified, " +
                                                "c.ContactId, " +
                                                "c.PoolId, " +
                                                "c.FirstName, " +
                                                "c.LastName, " +
                                                "c.ImageUrl, " +
                                                "c.Status, " +
                                                "c.DateLastPaid, " +
                                                "c.DateLastVisit, " +
                                                "c.AdditionalInformation, " +
                                                "c.Longitude, " +
                                                "c.Latitude, " +
                                                "c.Distance, " +
                                                "c.Balance, " +
                                                "c.SameHomeAddress, " +
                                                "c.Balance, " +
                                                "csch.CustomerIndex " +
                                                "FROM CustomerModel c JOIN CustomerSchedulerModel csch " +
                                                  "on c.Id = csch.CustomerId " +
                                                  "WHERE csch.SchedulerId = '" + schedulerId + "' " +
                                                  "Order By csch.CustomerIndex").ConfigureAwait(false);

                foreach (var customer in customers)
                {
                    var address = await new AddressController().LocalData.Load(customer.AddressId);
                    customer.Address = address;
                }

                return customers?.OrderBy(x=>x.CustomerIndex)?.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<CustomerModel>> SearchCustomer(string criteria)
        {
            try
            {
                var customers =  await SQLiteControllerBase
                    .DatabaseAsync
                    .QueryAsync<CustomerModel>("SELECT " +
                                                "c.Id, " +
                                                "c.FirstName, " +
                                                "c.LastName, " +
                                                "c.ImageUrl, " +
                                                "c.Active, " +
                                                "c.Status, " +
                                                "c.DateLastPaid, " +
                                                "c.DateLastVisit, " +
                                                "c.Balance, " +
                                                "c.Latitude, " +
                                                "c.Longitude, " +
                                                "c.AdditionalInformation, " +
                                                "c.Distance, " +
                                                "c.Created, " +
                                                "c.Modified, " +
                                                "c.WasModified, " +
                                                "c.AddressId, " +
                                                "c.ContactId, " +
                                                "c.PoolId, " +
                                                "c.HomeAddressId " +
                                                "FROM CustomerModel c " +
                                                  "LEFT OUTER JOIN AddressModel am on c.AddressId = am.Id " +
                                                  "LEFT OUTER JOIN AddressModel ha on c.HomeAddressId = ha.Id " +
                                                  "LEFT OUTER JOIN ContactModel cm on c.ContactId = cm.Id " +
                                                  "LEFT OUTER JOIN PoolModel pm    on c.PoolId = pm.Id " +
                                                  "WHERE c.FirstName like '%" + criteria + "%' " +
                                                  "OR c.LastName like '%" + criteria + "%' " +
                                                  "OR am.Address1 like '%" + criteria + "' " +
                                                  "OR am.City like '" + criteria + "%' " +
                                                  "OR am.State like '%" + criteria + "%' " +
                                                  "OR am.Zip like '%" + criteria + "%' " +
                                                  "OR ha.Address1 like '%" + criteria + "%' " +
                                                  "OR ha.City like '" + criteria + "%' " +
                                                  "OR ha.State like '%" + criteria + "%' " +
                                                  "OR ha.Zip like '%" + criteria + "%' " +
                                                  "OR cm.Phone like '%" + criteria + "%' " +
                                                  "OR cm.CellPhone like '%" + criteria + "%' " +
                                                  "OR cm.Email like '%" + criteria + "%' " +
                                                  "ORDER BY C.FirstName").ConfigureAwait(false);

                if (!customers.Any())
                {
                    customers = await SQLiteControllerBase
                                        .DatabaseAsync
                                        .QueryAsync<CustomerModel>("SELECT * " +
                                                                   "FROM CustomerModel " +
                                                                   "ORDER BY FirstName " +
                                                                   "LIMIT 10").ConfigureAwait(false);

                }
              
                foreach (var customer in customers)
                {
                    await SQLiteControllerBase
                    .DatabaseAsync
                    .GetChildrenAsync(customer, true)
                    .ConfigureAwait(false);

                    if (customer.Address == null)
                    {
                        customer.Address = new AddressModel();
                    }

                    if (customer.HomeAddress == null)
                    {
                        customer.HomeAddress = new AddressModel();
                    }
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

                    if (model.Address == null)
                    {
                        model.Address = new AddressModel();
                    }

                    if (model.HomeAddress == null)
                    {
                        model.HomeAddress = new AddressModel();
                    }
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
            var originalModel = JsonConvert.SerializeObject(model, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

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
                    if (model.Address != null)
                    {
                        model.Address.Id = model.AddressId = Guid.NewGuid();
                        model.Address.Created = created;
                        await new AddressController().LocalData.Modify(model.Address);
                    }

                    if (model.HomeAddress != null)
                    {
                        model.HomeAddress.Id = model.HomeAddressId = Guid.NewGuid();
                        model.HomeAddress.Created = created;
                        await new AddressController().LocalData.Modify(model.HomeAddress);
                    }

                    model.Contact.Id = model.ContactId = Guid.NewGuid(); 
                    model.Contact.CustomerId = model.Id;
                    model.Pool.Id =  model.PoolId = Guid.NewGuid();
                    model.Created = created;
                    model.Pool.Created = created;
                    model.Contact.Created = created;

                    // Add pool images
                    if (model?.Pool?.Images != null && model.Pool.Images.Any())
                    {
                        await new ImageController().LocalData.InsertAll(model.Pool.Images.Select(x => new EntityImageModel
                        {
                            EntityId = model.Pool.Id,
                            ImageUrl = x.ImageUrl,
                            ImageType = Enums.ImageType.Pool

                        }).ToList());
                    }

                    if (model?.Scheduler != null)
                    {
                        foreach (var scheduler in model.Scheduler)
                        {
                            if (scheduler.Customers != null)
                            {
                                scheduler.Customers.Add(model);
                            }
                        }
                    }
                }
                else 
                {
                    // Delete current images
                    await new ImageController().DeleteAllImagesAsync(model.Pool.Id, Enums.ImageType.Pool);

                    // Add new images
                    if (model?.Pool?.Images != null && model.Pool.Images.Any())
                    {
                        await new ImageController().LocalData.InsertAll(model.Pool.Images.Select(x => new EntityImageModel
                        {
                            EntityId = model.Pool.Id,
                            ImageUrl = x.ImageUrl,
                            ImageType = Enums.ImageType.Pool

                        }).ToList());
                    }

                    if (model?.Scheduler != null)
                    {
                        foreach (var scheduler in model.Scheduler)
                        {
                            if (scheduler.Customers != null)
                            {
                                scheduler.Customers.Add(model);
                            }
                        }
                    }

                    model.Modified = DateTime.Now.ToUniversalTime();
                }

                SQLiteNetExtensions
                    .Extensions
                    .WriteOperations
                    .InsertOrReplaceWithChildren(SQLiteControllerBase.DatabaseAsync.GetConnection(), model, true);
            }
            catch (Exception ex)
            {
                try
                {
                    model = JsonConvert.DeserializeObject<CustomerModel>(originalModel);
                }
                catch (Exception eex)
                {
                    Console.WriteLine(eex);
                }
                
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
                var m = await LocalData.List(new SQLControllerListCriteriaModel {
                  Filter = new List<SQLControllerListFilterField>
                  {
                      new SQLControllerListFilterField{
                          FieldName = "Id",
                          ValueLBound = id.ToString()
                      }
                  }});

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