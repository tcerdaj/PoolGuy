using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.ViewModels
{
    public class CustomerPageViewModel : BaseViewModel
    {
        IUserDialogs userDialogs;
        public CustomerPageViewModel()
        {
            userDialogs = DependencyService.Get<IUserDialogs>();
            SubscribeMessages();
        }

        private void SubscribeMessages()
        {
            Notify.SubscribePoolAction(async (sender) => {
                try
                {
                    Pool = await new PoolController().LoadAsync(Pool.Id);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
                }
            });
        }

        public bool ShowAddEquipment
        {
            get { return Pool.Id != Guid.Empty; }
        }

        private CustomerModel _customer = new CustomerModel() { };
        public CustomerModel Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private AddressModel _address = new AddressModel();
        public AddressModel Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged("Address"); }
        }

        private ObservableCollection<SchedulerModel> _schedulers = new ObservableCollection<SchedulerModel>();
        public ObservableCollection<SchedulerModel> Schedulers
        {
            get { return _schedulers; }
            set { _schedulers = value; OnPropertyChanged("Schedulers"); }
        }

        private ContactModel _contact = new ContactModel();
        public ContactModel Contact
        {
            get { return _contact; }
            set { _contact = value; OnPropertyChanged("Contact"); }
        }

        private PoolModel _pool = new PoolModel();
        public PoolModel Pool
        {
            get { return _pool; }
            set { _pool = value; OnPropertyChanged("Pool"); }
        }

        public string[] PoolTypes
        {
            get { return Enum.GetNames(typeof(PoolType)); }
        }

        public Page Page { get; set; }

        public void NotifyPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        public ICommand NextCommand
        {
            get => new RelayCommand<string>((control) => Next(control));
        }

        private void Next(string control)
        {
            try
            {
                var element = Page?.FindByName<object>(control);

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

        public ICommand GoToAddEquipmentCommand
        {
            get => new RelayCommand(() => GoToAddEquipment());
        }

        private async void GoToAddEquipment()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                await NavigationService.NavigateToDialog(new EquipmentPage(new EquipmentModel { PoolId = Pool.Id, Pool = Pool }) { 
                    Title = "Select Equipment" 
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand SelectEquipmentCommand
        {
            get => new RelayCommand<EquipmentModel>(async (model) => SelecteEquipment(model));
        }

        private async void SelecteEquipment(EquipmentModel model)
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                await NavigationService.NavigateToDialog(new EquipmentPage(model));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally 
            {
                IsBusy = false; 
            }
        }

        public ICommand DeleteEquipmentCommand
        {
            get => new RelayCommand<EquipmentModel>(async (model) => DeleteEquipment(model));
        }

        private async void DeleteEquipment(EquipmentModel model)
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                if (!await userDialogs.DisplayConfirmationAsync("Delete Confirmation", "Are you sure want to delete equipment?", "Delete", "Cancel"))
                {
                    return;
                }

                var obj = Pool.Equipments.FirstOrDefault(x=>x.Id == model.Id);
                Pool.Equipments.Remove(obj);
                await new PoolController().ModifyWithChildrenAsync(Pool);
                Pool.RaiseEquipmentNotification();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand GoToSchedulerCommand
        {
            get => new RelayCommand(() => GoToScheduler());
        }

        private async void GoToScheduler()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                await NavigationService.NavigateToDialog(Locator.Scheduler);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand CheckVisitingDayCommand
        {
            get => new RelayCommand<SchedulerModel>((m) => CheckVisitingDay(m));
        }

        private async void CheckVisitingDay(SchedulerModel model)
        {
            try
            {
                Notify.RaiseVisitingDayActionAction(new Messages.RefreshMessage {Object = model});
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
            }
        }
    }
}