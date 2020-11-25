using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EquipmentPage : ContentPage, IContentPage
    {
        EquipmentViewModel _viewModel;
        public EquipmentPage(EquipmentModel equipment)
        {
            InitializeComponent();
            _viewModel = new EquipmentViewModel(equipment) { Title = equipment.Id == Guid.Empty && Globals.CurrentPage == Enums.ePage.Equipment? "Select Equipment": "Update Equipment" };
            _viewModel.SetView(this);
            BindingContext = _viewModel;
        }

        public void CleanUp()
        {
            
        }

        public void Initialize()
        {
            
        }

        public MobileNavigationModel OnSleep()
        {
            return new MobileNavigationModel
            {
                CurrentPage = Locator.Equipment,
                PageViewModel = _viewModel,
                IsModal = true
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.InitEquipment();
            DateInstalledPicker.DateSelected += DateInstalledPicker_DateSelected;
            WarrantyExpirationPicker.DateSelected += DateInstalledPicker_DateSelected;
            LastMaintenancePicker.DateSelected += DateInstalledPicker_DateSelected;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            DateInstalledPicker.DateSelected -= DateInstalledPicker_DateSelected;
            WarrantyExpirationPicker.DateSelected -= DateInstalledPicker_DateSelected;
            LastMaintenancePicker.DateSelected -= DateInstalledPicker_DateSelected;
        }

        private void DateInstalledPicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                switch (datePicker.StyleId)
                {
                    case "DateInstalledPicker":
                        _viewModel.Equipment.DateInstalled = e.NewDate;
                        break;
                    case "WarrantyExpirationPicker":
                        _viewModel.Equipment.WarrantyExpiration = e.NewDate;
                        break;
                    case "LastMaintenancePicker":
                        _viewModel.Equipment.LastMaintenance = e.NewDate;
                        break;
                    default:
                        break;
                }
                
            }
        }
    }
}