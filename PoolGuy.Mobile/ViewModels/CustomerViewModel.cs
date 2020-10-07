
using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static PoolGuy.Mobile.Data.Models.Enums;

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

        public string[] PoolTypes
        {
            get { return Enum.GetNames(typeof(PoolType)); }
        }

        private string errorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged("ErrorMessage"); OnPropertyChanged("ErrorTextColor"); }
        }

        public string ErrorTextColor
        {
            get { return ErrorMessage.Contains("Unable") || ErrorMessage.Contains("Error") ? "Red" : "#009d00"; }
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
                
                var customerController = new CustomerController();

                var result = await customerController.ModifyAsync(Customer);

                ErrorMessage = result?.Status == Enums.eResultStatus.Ok? "Save Customer Success" :
                               $"Unable to save customer: {result?.Message}";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                ErrorMessage = $"Error: {e.Message}";
            }
        }

        public void OnPoolChanged()
        {
            OnPropertyChanged("Customer");
            Customer?.Pool?.RaiseAllNotification();
        }
    }
}