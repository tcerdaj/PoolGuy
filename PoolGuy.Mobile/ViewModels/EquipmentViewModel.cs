using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoolGuy.Mobile.ViewModels
{
    public class EquipmentViewModel : BaseViewModel
    {
        public EquipmentViewModel(EquipmentModel equipment)
        {
            Equipment = equipment;
            Initialize();
        }

        private void Initialize()
        {
            Device.BeginInvokeOnMainThread(async () => {
                if (Equipment != null && Equipment.PoolId != Guid.Empty)
                {
                    Pool = await new PoolController().LoadAsync(Equipment.PoolId);
                }
            });
        }

        private EquipmentModel _equipment;      

        public EquipmentModel Equipment
        {
            get { return _equipment; }
            set { _equipment = value; OnPropertyChanged("Equipment"); }
        }

        private PoolModel _pool;

        public PoolModel Pool
        {
            get { return _pool; }
            set { _pool = value; OnPropertyChanged("Pool"); }
        }

        public ICommand SaveCommand
        {
            get => new RelayCommand(async () => Save());
        }

        private async Task Save()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                if (Equipment == null)
                {
                    return;
                }

                // Is new equipment
                if (Equipment.Id == Guid.Empty)
                {
                    Pool.Equipments.Add(Equipment);
                }
                else
                {
                    var equipment = Pool.Equipments.FirstOrDefault(x => x.Id == Equipment.Id);
                    equipment = Equipment;
                    equipment.Modified = DateTime.Now;
                }

                await new PoolController().ModifyWithChildrenAsync(Pool);

                // Close current page

                // Notify refresh
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

        public ICommand GoBackCommand => new RelayCommand(async () =>
        {
            Notify.RaisePoolAction(new Messages.RefreshMessage());
            await NavigationService.PopPopupAsync(false);
            Shell.Current.SendBackButtonPressed();
        });
    }
}