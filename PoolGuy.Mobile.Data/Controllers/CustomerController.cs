using PoolGuy.Mobile.Data.Models;
using System;

namespace PoolGuy.Mobile.Data.Controllers
{
    public class CustomerController : BaseController<CustomerModel>
    {
        public CustomerController()
            :base()
        {
            
        }

        public async System.Threading.Tasks.Task<ResultStatus<CustomerModel>> ModifyAsync(CustomerModel customer)
        {
            ResultStatus<CustomerModel> result = null;

            try
            {
                if (customer == null)
                {
                    return result;
                }
                
                var poolController = new PoolController();
                await poolController.LocalData.CreateTableAsync();
                await LocalData.CreateTableAsync();
                
                if (customer.Id == Guid.Empty)
                {
                    DateTime created = DateTime.Now;
                    customer.Id = Guid.NewGuid();
                    customer.Created = created;

                    if (customer.Pool != null)
                    {
                        customer.Pool.Created = created;
                        customer.Pool.Name = customer.Name;
                        customer.Pool.Description = $"Customer {customer.Name}, localte in {customer.Address1}";
                        customer.Pool = await poolController.LocalData.Modify(customer.Pool);
                        customer.PoolID = customer.Pool.Id;
                    }
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

        public async System.Threading.Tasks.Task<ResultStatus<CustomerModel>> DeleteAsync(CustomerModel customer)
        {
            ResultStatus<CustomerModel> result = null;

            try
            {
                if (customer == null)
                {
                    return result;
                }

                var poolController = new PoolController();
                await poolController.LocalData.ClearTableAsync();
                await LocalData.CreateTableAsync();

                await poolController.LocalData.Delete(customer.Pool);
                await LocalData.Delete(customer);
                result = new ResultStatus<CustomerModel>(Enums.eResultStatus.Ok, string.Empty, customer);
            }
            catch (System.Exception e)
            {
                result = new ResultStatus<CustomerModel>(Enums.eResultStatus.Error, e.Message, customer);
            }

            return result;
        }
    }
}