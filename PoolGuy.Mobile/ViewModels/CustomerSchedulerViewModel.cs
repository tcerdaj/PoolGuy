using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoolGuy.Mobile.ViewModels
{
    public class CustomerSchedulerViewModel : BaseViewModel
    {
        public CustomerSchedulerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "").Replace("Search", "");
            Globals.CurrentPage = Enums.ePage.Scheduler;
        }

        private string _searchTerm = "";
        public string SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; }
        }

        private CustomerModel _customer = new CustomerModel();
        public CustomerModel Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private ObservableCollection<CustomerModel> _customers = new ObservableCollection<CustomerModel>();
        public ObservableCollection<CustomerModel> Customers
        {
            get { return _customers; }
            set { _customers = value; OnPropertyChanged("Customers"); }
        }

        private ObservableCollection<CustomerModel> _allCustomers = new ObservableCollection<CustomerModel>();
        public ObservableCollection<CustomerModel> CustomerSearchResults
        {
            get { return _allCustomers; }
            set { _allCustomers = value; OnPropertyChanged("CustomerSearchResults"); }
        }

        private ObservableCollection<SchedulerModel> _schedulers = new ObservableCollection<SchedulerModel>();
        public ObservableCollection<SchedulerModel> Schedulers
        {
            get { return _schedulers; }
            set { _schedulers = value; OnPropertyChanged("Schedulers"); }
        }

        public Page CurrentPage
        {
            get => (Shell.Current?.CurrentItem?.CurrentItem as IShellSectionController)?.PresentedPage;
        }

        public async Task InitializeAsync()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                if (Schedulers != null && Schedulers.Any())
                {
                    List<CustomerModel> customers = null;
                    var selectedSchedule = Schedulers
                        .FirstOrDefault(x => x.Selected);

                    if(selectedSchedule != null)
                    {
                        customers = selectedSchedule.Customers;
                    }
                   
                    if (customers != null && customers.Any())
                    {
                        var cust = Customers.ToList();
                        cust.AddRange(customers);
                        Customers = new ObservableCollection<CustomerModel>(cust);
                    }
                    
                    CustomerSearchResults = Customers;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
            finally { IsBusy = false; }
        }

        public ICommand SaveCommand 
        {
            get => new RelayCommand(async() => Save());
        }

        private async void Save()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                //if (!FieldValidationHelper.IsFormValid(Scheduler, CurrentPage))
                //{
                //    return;
                //}

                //await new SchedulerController().LocalData.Modify(Scheduler);

                //// Add/modify list
                //if (!Schedulers.Any(x => x.Id == Scheduler.Id))
                //{
                //    Schedulers.Insert(Scheduler.Index, Scheduler);
                //    OnPropertyChanged("Schedulers");
                //}
                //else
                //{
                //    var sch = Schedulers.FirstOrDefault(x => x.Id == Scheduler.Id);
                    
                //    if(sch != null)
                //    {
                //        sch = Scheduler;
                //        Schedulers = new ObservableCollection<SchedulerModel>(Schedulers.OrderBy(x => x.Index));
                //    }
                //}
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
            finally { IsBusy = false; }
        }

        public ICommand GoToCustomerCommand
        {
            get => new RelayCommand<SchedulerModel>(async (scheduler) => GoToCustomer(scheduler));
        }

        private async void GoToCustomer(SchedulerModel scheduler)
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                await Shell.Current.DisplayAlert(Title, $"Go To Customers scheduler:{scheduler.LongName}", "Ok");
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
                if (string.IsNullOrEmpty(SearchTerm))
                {
                    CustomerSearchResults = Customers;
                }
                else
                {
                    CustomerSearchResults = new ObservableCollection<CustomerModel>(await new CustomerController().SearchCustomer(SearchTerm));
                }
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

        public ICommand UnSelectAllCommand
        {
            get => new RelayCommand(async () => UnSelectAll());
        }

        private void UnSelectAll()
        {
            throw new NotImplementedException();
        }

        public ICommand SelectAllCommand
        {
            get => new RelayCommand(async () => SelectAll());
        }

        private void SelectAll()
        {
            throw new NotImplementedException();
        }
    }
}