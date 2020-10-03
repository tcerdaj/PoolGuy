
using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoolGuy.Mobile.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        private Page _page;
        public CustomerViewModel(Page page)
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
            _page = page;
        }

        private CustomerModel _customer = new CustomerModel();

        public CustomerModel Customer
        {
            get { return _customer; } 
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private string errorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged("ErrorMessage");}
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(async ()=>await SaveCustomerAsync());
            }
        }

        private async Task SaveCustomerAsync()
        {
            try
            {
                if (!FieldValidationHelper.IsFormValid(Customer, _page)) 
                {
                    ErrorMessage = "Unable to save customer";
                    return;
                }

                ErrorMessage = "";

                await Message.DisplayAlertAsync("Save Customer Success", "Success");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}