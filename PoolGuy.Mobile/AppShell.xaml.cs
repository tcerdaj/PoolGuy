using System;
using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Helpers;
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
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                var menuItem = sender as MenuItem;
                if (menuItem == null)
                {
                    return;
                }

                var selectedItem = menuItem.Text;
                switch (selectedItem)
                {
                    case "Logout":
                        Application
                            .Current
                            .MainPage = new LoginPage()
                            {
                                BackgroundColor = Color.White
                            };
                        Settings.IsLoggedIn = false;
                        break;
                    case "Settings":
                        await Current.GoToAsync(Locator.Settings);
                        break;
                    default:
                        await Current.DisplayAlert(Title, $"Unable to navigate to {selectedItem} page", "Ok");
                        break;
                }

                this.FlyoutIsPresented = false;
            }
            catch (Exception ex)
            {
                await Current.DisplayAlert("Error", $"Unable to logout: {ex.Message}", "Ok");
            }
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {

        }

        private void MenuItem_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}
