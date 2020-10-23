using CommonServiceLocator;
using PoolGuy.Mobile.Data.Models;
using Xamarin.Forms;

namespace PoolGuy.Mobile.ViewModels
{
    public class SearchCustomerViewModel : BaseViewModel
    {
        public SearchCustomerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
        }

        private CustomerModel _customer = new CustomerModel() {Pool = new PoolModel()};

        public CustomerModel Customer
        {
            get { return _customer; } 
            set { _customer = value; OnPropertyChanged("Customer"); }
        }
    }
}