using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoolGuy.Mobile.ViewModels
{
    public class SchedulerViewModel : BaseViewModel
    {
        public SchedulerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "").Replace("Search", "");
            Globals.CurrentPage = Enums.ePage.Scheduler;
            InitializeAsync();
        }

        private string _searchTerm = "";
        public string SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; }
        }

        private SchedulerModel _scheduler;

        public SchedulerModel Scheduler
        {
            get { return _scheduler; }
            set { _scheduler = value; OnPropertyChanged("Scheduler"); }
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

        public ICommand GoToSchedulerDetailsCommand
        {
            get { return new RelayCommand<SchedulerModel>(async (scheduler) => await GoToDetails(scheduler)); }
        }

        private async Task GoToDetails(SchedulerModel scheduler)
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                //await Shell.Current.Navigation.PushAsync(new WizardCustomerPage(customer) { Title = "Customer" });
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

        public async Task InitializeAsync()
        {
            try
            {
                Reset();  
                 
                Schedulers = new ObservableCollection<SchedulerModel>(await new SchedulerController().LocalData.List());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
        }

        public ICommand GoBackCommand => new RelayCommand(async () =>
        {
            try
            {
              
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
        });

        public ICommand SaveCommand 
        {
            get => new RelayCommand(async() => Save());
        }

        private async void Save()
        {
            try
            {
                if (!FieldValidationHelper.IsFormValid(Scheduler, CurrentPage))
                {
                    return;
                }

                await new SchedulerController().LocalData.Modify(Scheduler);
                Schedulers.Add(Scheduler);
                OnPropertyChanged("Schedulers");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
        }

        public ICommand GoToCustomerCommand
        {
            get => new RelayCommand(async () => GoToCustomer());
        }

        private async void GoToCustomer()
        {
            await Shell.Current.DisplayAlert(Title,"Go To Customers","Ok");
        }

        public ICommand ResetCommand
        {
            get => new RelayCommand(async () => Reset());
        }

        private void Reset()
        {
            Scheduler = new SchedulerModel()
            {
                User = new UserModel
                {
                    Id = Guid.Parse("9fceac3f-0b57-423c-abcf-2b214dd7af6f"),
                    FirstName = "Teofilo",
                    LastName = "Cerda"
                },
                Customers = new List<CustomerModel>()
            };
        }
    }
}