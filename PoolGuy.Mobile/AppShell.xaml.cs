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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
