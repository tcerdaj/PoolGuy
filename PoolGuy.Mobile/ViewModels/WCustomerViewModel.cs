using PoolGuy.Mobile.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoolGuy.Mobile.ViewModels
{
    public class WCustomerViewModel : BaseViewModel
    {
        public WCustomerViewModel()
        {
            Title = "Customer";
        }
        private CustomerModel _customer = new CustomerModel() { };
        public CustomerModel Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }
    }
}
