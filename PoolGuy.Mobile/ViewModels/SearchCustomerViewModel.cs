using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoolGuy.Mobile.ViewModels
{
    public class SearchCustomerViewModel : BaseViewModel
    {
        public SearchCustomerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "").Replace("Search", "");
        }

        private string _searchTerm = "";
        public string SearchTerm
        {
            get { return _searchTerm; }
            set {_searchTerm = value; }
        }

        private List<CustomerModel> _customers = new List<CustomerModel>();
        public List<CustomerModel> Customers
        {
            get { return _customers; }
            set { _customers = value; OnPropertyChanged("Customers"); }
        }

        public ICommand GoToCustomerDetailsCommand
        {
            get { return new RelayCommand<CustomerModel>(async (customer) => await GoToCustomerDetails(customer)); }
        }

        private Task GoToCustomerDetails(CustomerModel customer)
        {
            throw new NotImplementedException();
        }

        public ICommand AddWorkCommand
        {
            get { return new RelayCommand<CustomerModel>(async (customer) => await AddWork(customer)); }
        }

        private Task AddWork(CustomerModel customer)
        {
            throw new NotImplementedException();
        }

        public ICommand ScheduleCustomerCommand
        {
            get { return new RelayCommand<CustomerModel>(async (customer) => await ScheduleCustomer(customer)); }
        }

        private Task ScheduleCustomer(CustomerModel customer)
        {
            throw new NotImplementedException();
        }

        public ICommand DeleteCustomerCommand
        {
            get { return new RelayCommand<CustomerModel>(async(customer) => await DeleteCustomer(customer));}
        }

        private async Task DeleteCustomer(CustomerModel customer)
        {
            try
            {
                if (await Shell.Current.DisplayAlert("Confirmation", "Are you sure want to delete customer?", "Delete", "Cancel").ConfigureAwait(false))
                {
                   await new CustomerController().DeleteAsync(customer);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(async () => await AddAsync()); }
        }

        private async Task AddAsync()
        {
            await Shell.Current.GoToAsync("WizardCustomerPage");
        }

        public ICommand SearchCustomerCommand
        {
            get { return new RelayCommand(async () => await SearchCustomer()); }
        }

        private async Task SearchCustomer()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                Customers = await new CustomerController().SearchCustomer(SearchTerm);
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}