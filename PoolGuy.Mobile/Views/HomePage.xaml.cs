using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Views
{
    public partial class HomePage : ContentPage, IContentPage
    {
        HomeViewModel _viewModel;
        public HomePage()
        {
            InitializeComponent();
            Globals.CurrentPage = Data.Models.Enums.ePage.Home;
            _viewModel = new HomeViewModel() { IsBusy = false };
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
                CurrentPage = Locator.Home,
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

            if (!_viewModel.Initialized)
            {
                await _viewModel.Initialize();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}