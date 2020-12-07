using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace PoolGuy.Mobile.ViewModels
{
    public class CustomerSchedulerViewModel : BaseViewModel
    {
        public CustomerSchedulerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "").Replace("Search", "");
            Globals.CurrentPage = Enums.ePage.Scheduler;
        }

        public Guid SelectedSchedulerId { get; set; }

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

        public async Task InitSchedulers()
        {
            Schedulers = new ObservableCollection<SchedulerModel>(await new SchedulerController().ListWithChildrenAsync(new Data.Models.Query.SQLControllerListCriteriaModel { 
             Sort = new List<Data.Models.Query.SQLControllerListSortField> { 
              new Data.Models.Query.SQLControllerListSortField{ FieldName = "Index"}
             }}));
      
            var sch = Schedulers.FirstOrDefault(x => x.Id == SelectedSchedulerId);

            if (sch != null)
            {
                sch.Selected = true;
                sch.Customers.ForEach((c) => { c.Selected = true; });
            }
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

        public void RefreshList()
        {
            var customers = Schedulers.Where(c => c.Selected)
                .SelectMany(p => p.Customers)
                .GroupBy(g => g.Id)
                .Select(m =>m.First())
                .Select(x => { x.Selected = true; return x; })
                .ToList();

            CustomerSearchResults = new ObservableCollection<CustomerModel>(customers);
        }

        public ICommand SaveCommand 
        {
            get => new RelayCommand(async() => Save());
        }

        private async void Save()
        {
            if (IsBusy) { return; }
            IsBusy = true;
            string customers = string.Empty;

            try
            {
                if (!Schedulers.Where(x => x.Selected).Any())
                {
                    await Shell.Current.DisplayAlert(Title, "Please make a selection first", "Ok");
                    return;
                }

                var selectedCustomers = CustomerSearchResults
                    .Where(x => x.Selected)
                    .ToList();

                customers = JsonConvert.SerializeObject(Customers, Formatting.Indented,
                            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

                var distinctCustomers = Customers
                      .Where(x=>x.Selected)
                      .GroupBy(g => g.Id)
                      .Select(m => m.First());

                selectedCustomers.AddRange(distinctCustomers);
                Customers.Clear();

                foreach (var sch in Schedulers.Where(x=>x.Selected))
                {
                    
                    sch.Customers = selectedCustomers;
                    await new SchedulerController().ModifyWithChildrenAsync(sch);
                    
                    selectedCustomers.ForEach((c) =>
                    {
                        if (!Customers.Any(x => x.Id == c.Id))
                            Customers.Add(c);
                    });
                }

                CustomerSearchResults = Customers;
                OnPropertyChanged("CustomerSearchResults");
                OnPropertyChanged("Schedulers");

                Message.Toast($"{selectedCustomers.Count()} Customers were added successfully!");
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(customers))
                {
                    Customers = JsonConvert.DeserializeObject<ObservableCollection<CustomerModel>>(customers);
                }
                
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
                    var list = await new CustomerController().SearchCustomer(SearchTerm);
                    CustomerSearchResults = new ObservableCollection<CustomerModel>(list.Where(x => !Customers.Any(p => p.Id == x.Id)));
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

        public ICommand SelectCommand
        {
            get => new RelayCommand<bool>(async (all) => Select(all));
        }

        private void Select(bool all)
        {
            CustomerSearchResults = new ObservableCollection<CustomerModel>(
                CustomerSearchResults.Select(x => { x.Selected = all; return x; }));
        }
    }
}