using PoolGuy.Mobile.Data.Models;
using System;

namespace PoolGuy.Mobile.Data.Controllers
{
    public class CustomerController : BaseController<Customer>
    {
        public CustomerController()
            :base()
        {
            
        }

        public async System.Threading.Tasks.Task<ResultStatus<Customer>> ModifyAsync(Customer customer)
        {
            ResultStatus<Customer> result = null;

            try
            {
                if (customer == null)
                {
                    return result;
                }
                
                var poolController = new PoolController();
                await poolController.LocalData.ClearTableAsync();
                await LocalData.CreateTableAsync();
                
                if (customer.Id == Guid.Empty)
                {
                    DateTime created = DateTime.Now;
                    customer.Id = Guid.NewGuid();
                    customer.Created = created;

                    if (customer.Pool != null)
                    {
                        customer.Pool.Created = created;
                        customer.Pool = await poolController.LocalData.Modify(customer.Pool);
                    }
                }
                else
                {
                    customer.Modified = DateTime.Now;
                }

                result = new ResultStatus<Customer>(Enums.eResultStatus.Ok, 
                    string.Empty, await LocalData.Modify(customer));
            }
            catch (System.Exception e)
            {
                result = new ResultStatus<Customer>(Enums.eResultStatus.Error, e.Message, customer);
            }

            return result;
        }

        public async System.Threading.Tasks.Task<ResultStatus<Customer>> DeleteAsync(Customer customer)
        {
            ResultStatus<Customer> result = null;

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
                result = new ResultStatus<Customer>(Enums.eResultStatus.Ok, string.Empty, customer);
            }
            catch (System.Exception e)
            {
                result = new ResultStatus<Customer>(Enums.eResultStatus.Error, e.Message, customer);
            }

            return result;
        }
    }
}