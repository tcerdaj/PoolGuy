using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.ViewModels
{
    public class CustomerPageViewModel : BaseViewModel
    {
        public CustomerPageViewModel()
        {
            SubscribeMessages();
        }

        private void SubscribeMessages()
        {
            Notify.SubscribePoolAction(async (sender) => {
                Pool = await new PoolController().LoadAsync(Pool.Id);
            });
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
                await Shell.Current.Navigation.PushAsync(new EquipmentPage(new EquipmentModel { PoolId = Pool.Id }) { Title = "Select Equipment" });
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
                await Shell.Current.Navigation.PushAsync(new EquipmentPage(model));
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
                if (!await Shell.Current.DisplayAlert("Delete Confirmation", "Are you sure want to delete equipment?", "Delete", "Cancel"))
                {
                    return;
                }

                Pool.Equipments.Remove(model);
                await new PoolController().ModifyWithChildrenAsync(Pool);
                Pool.RaiseEquipmentNotification();
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
    }
}