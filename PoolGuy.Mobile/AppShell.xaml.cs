using System;
using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Views;
using Xamarin.Forms;

namespace PoolGuy.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(CustomerPage), typeof(CustomerPage));
            Routing.RegisterRoute(nameof(SearchCustomerPage), typeof(SearchCustomerPage));
            Routing.RegisterRoute(nameof(ActionSheetPopupPage), typeof(ActionSheetPopupPage));
            Routing.RegisterRoute(nameof(EquipmentPage), typeof(EquipmentPage));
            Routing.RegisterRoute(nameof(WizardCustomerPage), typeof(WizardCustomerPage));
            Routing.RegisterRoute(nameof(SchedulerPage), typeof(SchedulerPage));
            Routing.RegisterRoute(nameof(WorkOrderDetailsPage), typeof(WorkOrderDetailsPage));
            Routing.RegisterRoute(nameof(WorkOrderListPage), typeof(WorkOrderListPage));
            Routing.RegisterRoute(nameof(CustomerSchedulerPage), typeof(CustomerSchedulerPage));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                Application.Current.MainPage = new LoginPage() { BackgroundColor = Color.White };
                Settings.IsLoggedIn = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Unable to logout: {ex.Message}", "Ok");
            }
        }
    }
}
