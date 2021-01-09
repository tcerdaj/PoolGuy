using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Extensions;

namespace PoolGuy.Mobile.ViewModels
{
    public class StopDetailsViewModel : BaseViewModel
    {
        public StopDetailsViewModel(CustomerModel customer)
        {
            Title = this.GetType().Name.Replace("ViewModel", "").SplitWord();
            Globals.CurrentPage = Enums.ePage.StopDetails;
            Customer = customer;
            SubscribeMessage();
            IsBusy = true;
        }

        private CustomerModel _customer;
        public CustomerModel Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private void SubscribeMessage()
        {
         
        }
    }
}