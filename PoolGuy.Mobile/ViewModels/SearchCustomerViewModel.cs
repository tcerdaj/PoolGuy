using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Services.Interface;

namespace PoolGuy.Mobile.ViewModels
{
    public class SearchCustomerViewModel : BaseViewModel
    {
        public SearchCustomerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "").Replace("Search", "");
            SubscribeMessages();
        }

        private void SubscribeMessages()
        {
            Notify.SubscribeSearchCustomerAction((sender) => {
                SearchCustomerCommand.Execute(null);
            });
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

        public ICommand GoToContactCommand
        {
            get { return new RelayCommand<CustomerModel>(async (customer) => await GoToContact(customer)); }
        }

        private async Task GoToContact(CustomerModel customer)
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                var action = await Message.DisplayActionSheetAsync("Select an option", "Cancel", "Call", "Text", "Email");
                if (string.IsNullOrEmpty(action) || action == "Cancel")
                {
                    return;
                }

                switch (action)
                {
                    case "Call":
                        PhoneDialer.Open(customer.Contact.Phone);
                        break;
                    case "Text":
                        await Sms.ComposeAsync(new SmsMessage("", customer.Contact.Phone));
                        break;
                    case "Email":
                        await Email.ComposeAsync("", "", customer.Contact.Email);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
            finally { IsBusy = false; }
        }

        public ICommand GoToCustomerDetailsCommand
        {
            get { return new RelayCommand<CustomerModel>(async (customer) => await GoToCustomerDetails(customer)); }
        }

        private async Task GoToCustomerDetails(CustomerModel customer)
        {
            if (IsBusy){ return; }
            IsBusy = true;

            try
            {
                await Shell.Current.Navigation.PushAsync(new WizardCustomerPage(customer) { Title = "Customer"});
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
            finally 
            {
                IsBusy = false;
            }
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
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                if (await Shell.Current.DisplayAlert("Confirmation", $"Are you sure want to delete {customer.Name} customer?", "Delete", "Cancel").ConfigureAwait(false))
                {
                   await new CustomerController().DeleteAsync(customer);
                   Customers = Customers.Where(x => x.Id != customer.Id).ToList();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
            finally { IsBusy = false; }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(async () => await AddAsync()); }
        }

        private async Task AddAsync()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                await Shell.Current.GoToAsync("WizardCustomerPage");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
            finally { IsBusy = false; }
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