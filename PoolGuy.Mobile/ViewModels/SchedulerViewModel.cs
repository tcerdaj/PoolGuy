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
using Newtonsoft.Json;
using PoolGuy.Mobile.CustomControls;

namespace PoolGuy.Mobile.ViewModels
{
    public class SchedulerViewModel : BaseViewModel
    {
        public SchedulerViewModel()
        {
            if (Globals.CurrentPage != Enums.ePage.Scheduler)
            {
                Globals.CurrentPage = Enums.ePage.Scheduler;
            }

            Title = this.GetType().Name.Replace("ViewModel", "").Replace("Search", "");
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
            get => NavigationService.CurrentPage;
        }

        public async Task InitializeAsync()
        {
            if(IsBusy){ return; }
            IsBusy = true;

            try
            {
                Schedulers = new ObservableCollection<SchedulerModel>(await new SchedulerController()
                    .ListWithChildrenAsync(new Data.Models.Query.SQLControllerListCriteriaModel {
                     Sort = new List<Data.Models.Query.SQLControllerListSortField> { 
                        new Data.Models.Query.SQLControllerListSortField {
                          FieldName = "Index"
                        }
                     }}));

                Reset();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
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
                if (!FieldValidationHelper.IsFormValid(Scheduler, CurrentPage))
                {
                    await Message.DisplayAlertAsync("Please check your enter and try again", Title);
                    return;
                }

                await new SchedulerController().ModifyWithChildrenAsync(Scheduler);

                // Add/modify list
                if (!Schedulers.Any(x => x.Id == Scheduler.Id))
                {
                    Schedulers.Insert(Scheduler.Index, Scheduler);
                    Reset();
                    OnPropertyChanged("Schedulers");
                }
                else
                {
                    var sch = Schedulers.FirstOrDefault(x => x.Id == Scheduler.Id);
                    
                    if(sch != null)
                    {
                        sch = Scheduler;
                        Schedulers = new ObservableCollection<SchedulerModel>(Schedulers.OrderBy(x => x.Index));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally { IsBusy = false; }
        }

        public ICommand DeleteCommand
        {
            get => new RelayCommand<SchedulerModel>((scheduler) => DeleteAsync(scheduler));
        }

        private async void DeleteAsync(SchedulerModel scheduler)
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                if (await Message.DisplayConfirmationAsync("Confirmation", $"Are you sure want to delete {scheduler.LongName} scheduler?", "Delete", "Cancel").ConfigureAwait(false))
                {
                    await new SchedulerController().DeleteAsync(scheduler);
                    Schedulers = new ObservableCollection<SchedulerModel>(Schedulers.Where(x => x.Id != scheduler.Id).OrderBy(x=>x.Index));
                    // Get next index
                    Scheduler.IncreaseIndex(Schedulers.Max(x => x.Index));
                    OnPropertyChanged("Scheduler");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
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
                await NavigationService.NavigateToDialog(Locator.CustomerScheduler, scheduler.Id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally { IsBusy = false; }
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
                Customers = new List<CustomerModel>(),
                Index = Schedulers.Any()? Schedulers.Max(x => x.Index) + 1: 0
            };
        }

        public ICommand NextCommand
        {
            get => new RelayCommand<string>((control) => Next(control));
        }

        private void Next(string control)
        {
            try
            {
                var element = CurrentPage?.FindByName<object>(control);

                if (element is CustomEntry customEntry)
                {
                    customEntry.Focus();
                }

                if (element is Editor editor)
                {
                    editor.Focus();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public ICommand NavigateToCommand
        {
            get
            {
                return new RelayCommand<Enums.ePage>(async (item) =>
                {
                    string page = item == Enums.ePage.Customer ? $"Search{item.ToString()}" : item.ToString();
                    await NavigationService.ReplaceRoot($"{page}Page");
                });
            }
        }
    }
}