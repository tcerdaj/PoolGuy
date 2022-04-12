using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Views
{
    public partial class StopsPage : ContentPage, IContentPage
    {
        StopsViewModel _viewModel;
        public StopsPage()
        {
            InitializeComponent();
            _viewModel = new StopsViewModel();
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
                CurrentPage = Locator.Stops,
                //PageViewModel = _viewModel,
                IsModal = true
            };
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!Settings.IsLoggedIn)
            {
                Application.Current.MainPage = new LoginPage() { BackgroundColor = Color.White };
                return;
            }

            await _viewModel.RefreshStopsAsync();
            _viewModel.IsBusy = false;
        }
    }
}