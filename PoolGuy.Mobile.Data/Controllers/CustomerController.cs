using PoolGuy.Mobile.Data.Models;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PoolGuy.Mobile.Data.Controllers
{
    public class CustomerController : BaseController<CustomerModel>
    {
        public CustomerController()
            :base()
        {
            
        }

        public async Task<ResultStatus<PoolModel>> ModifyPoolAsync(PoolModel model, string customerName)
        {
            ResultStatus<PoolModel> result = null;

            try
            {
                if (model == null)
                {
                    return result;
                }
                
                var controller = new PoolController();
                await controller.LocalData.CreateTableAsync();

                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                    model.Created = DateTime.Now;
                    model.Name = customerName;
                    model.Description = $"Customer {customerName}";
                }
                else
                {
                    model.Modified = DateTime.Now;
                }

                result = new ResultStatus<PoolModel>(Enums.eResultStatus.Ok,
                    string.Empty, await controller.LocalData.Modify(model));
            }
            catch (System.Exception e)
            {
                result = new ResultStatus<PoolModel>(Enums.eResultStatus.Error, e.Message, model);
            }

            return result;
        }

        public async Task<ResultStatus<PoolModel>> DeletePoolAsync(PoolModel model)
        {
            ResultStatus<PoolModel> result = null;

            try
            {
                if (model == null)
                {
                    return result;
                }

                var controller = new PoolController();
                await controller.LocalData.CreateTableAsync();
                await controller.LocalData.Delete(model);

                result = new ResultStatus<PoolModel>(Enums.eResultStatus.Ok,
                    string.Empty, null);

            }
            catch (Exception e)
            {
                result = new ResultStatus<PoolModel>(Enums.eResultStatus.Error, e.Message, model);
            }

            return result;
        }

        public async Task<ResultStatus<AddressModel>> ModifyAddressAsync(AddressModel model)
        {
            ResultStatus<AddressModel> result = null;
            
            try
            {
                if (model == null)
                {
                    return result;
                }

                var controller = new AddressController();
                await controller.LocalData.CreateTableAsync();

                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                    model.Created = DateTime.Now;
                }
                else
                {
                    model.Modified = DateTime.Now;
                }

                result = new ResultStatus<AddressModel>(Enums.eResultStatus.Ok,
                    string.Empty, await controller.LocalData.Modify(model));
            }
            catch (Exception e)
            {
                result = new ResultStatus<AddressModel>(Enums.eResultStatus.Error, e.Message, model);
            }

            return result;
        }

        public async Task<ResultStatus<AddressModel>> DeleteAddressAsync(AddressModel model)
        {
            ResultStatus<AddressModel> result = null;

            try
            {
                if (model == null)
                {
                    return result;
                }

                var controller = new AddressController();
                await controller.LocalData.CreateTableAsync();
                await controller.LocalData.Delete(model);
             
                result = new ResultStatus<AddressModel>(Enums.eResultStatus.Ok,
                    string.Empty, null);

            }
            catch (Exception e)
            {
                result = new ResultStatus<AddressModel>(Enums.eResultStatus.Error, e.Message, model);
            }

            return result;
        }

        public async Task<ResultStatus<ContactInformationModel>> ModifyContactInformationAsync(ContactInformationModel model)
        {
            ResultStatus<ContactInformationModel> result = null;

            try
            {
                if (model == null)
                {
                    return result;
                }

                var controller = new ContactInformationController();
                await controller.LocalData.CreateTableAsync();

                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                    model.Created = DateTime.Now;
                }
                else
                {
                    model.Modified = DateTime.Now;
                }

                result = new ResultStatus<ContactInformationModel>(Enums.eResultStatus.Ok,
                    string.Empty, await controller.LocalData.Modify(model));
            }
            catch (Exception e)
            {
                result = new ResultStatus<ContactInformationModel>(Enums.eResultStatus.Error, e.Message, model);
            }

            return result;
        }

        public async Task<ResultStatus<ContactInformationModel>> DeleteContactInformationAsync(ContactInformationModel model)
        {
            ResultStatus<ContactInformationModel> result = null;

            try
            {
                if (model == null)
                {
                    return result;
                }

                var controller = new ContactInformationController();
                await controller.LocalData.CreateTableAsync();
                await controller.LocalData.Delete(model);

                result = new ResultStatus<ContactInformationModel>(Enums.eResultStatus.Ok,
                    string.Empty, null);

            }
            catch (Exception e)
            {
                result = new ResultStatus<ContactInformationModel>(Enums.eResultStatus.Error, e.Message, model);
            }

            return result;
        }

        public async Task<ResultStatus<CustomerModel>> ModifyAsync(CustomerModel customer)
        {
            ResultStatus<CustomerModel> result = null;

            try
            {
                if (customer == null)
                {
                    return result;
                }
                
                await LocalData.CreateTableAsync();
                
                if (customer.Id == Guid.Empty)
                {
                    customer.Id = Guid.NewGuid();
                    customer.Created = DateTime.Now;
                }
                else
                {
                    customer.Modified = DateTime.Now;
                }

                result = new ResultStatus<CustomerModel>(Enums.eResultStatus.Ok, 
                    string.Empty, await LocalData.Modify(customer));
            }
            catch (System.Exception e)
            {
                result = new ResultStatus<CustomerModel>(Enums.eResultStatus.Error, e.Message, customer);
            }

            return result;
        }

        public async Task<ResultStatus<CustomerModel>> DeleteAsync(CustomerModel customer)
        {
            ResultStatus<CustomerModel> result = null;

            try
            {
                if (customer == null)
                {
                    return result;
                }

                // Remove address
                await new AddressController().LocalData.Delete(customer.Address);

                // Remove contact information
                await new ContactInformationController().LocalData.Delete(customer.ContactInformation);

                // Remove pool
                await new PoolController().LocalData.Delete(customer.Pool);

                // Remove customer
                await LocalData.Delete(customer);
                
                result = new ResultStatus<CustomerModel>(Enums.eResultStatus.Ok, string.Empty, customer);
            }
            catch (System.Exception e)
            {
                result = new ResultStatus<CustomerModel>(Enums.eResultStatus.Error, e.Message, customer);
            }

            return result;
        }

        public async Task<ResultStatus<CustomerModel>> LoadAsync(Guid id)
        {
            ResultStatus<CustomerModel> result = null;

            try
            {
                if (id.Equals(Guid.Empty))
                {
                    return result;
                }

                // load customer
                var model = await LocalData.Load(id);

                // load address
                var address = await new AddressController()
                    .LocalData
                    .List(new Models.Query.SQLControllerListCriteriaModel {
                        View = nameof(AddressModel),
                        Filter = new System.Collections.Generic.List<Models.Query.SQLControllerListFilterField> { 
                         new Models.Query.SQLControllerListFilterField { FieldName = "CustomerId", ValueLBound = model.Id.ToString()}
                       }
                    }).ConfigureAwait(false);

                model.Address = address.FirstOrDefault();

                // load contact information
                var contact = await new ContactInformationController()
                    .LocalData
                    .List(new Models.Query.SQLControllerListCriteriaModel
                    {
                        View = nameof(ContactInformationModel),
                        Filter = new System.Collections.Generic.List<Models.Query.SQLControllerListFilterField> {
                         new Models.Query.SQLControllerListFilterField { FieldName = "CustomerId", ValueLBound = model.Id.ToString()}
                       }
                    }).ConfigureAwait(false);

                model.ContactInformation = contact.FirstOrDefault();

                // load pool
                var pool = await new PoolController()
                    .LocalData
                    .List(new Models.Query.SQLControllerListCriteriaModel
                    {
                        View = nameof(PoolModel),
                        Filter = new System.Collections.Generic.List<Models.Query.SQLControllerListFilterField> {
                         new Models.Query.SQLControllerListFilterField { FieldName = "CustomerId", ValueLBound = model.Id.ToString()}
                       }
                    }).ConfigureAwait(false);


                result = new ResultStatus<CustomerModel>(Enums.eResultStatus.Ok, string.Empty, model);
            }
            catch (System.Exception e)
            {
                result = new ResultStatus<CustomerModel>(Enums.eResultStatus.Error, e.Message, null);
            }

            return result;
        }

    }
}