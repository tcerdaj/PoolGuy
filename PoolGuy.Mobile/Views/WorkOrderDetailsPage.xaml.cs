using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Views
{
    public partial class WorkOrderDetailsPage : ContentPage, IContentPage
    {
        HomeViewModel _viewModel;
        public WorkOrderDetailsPage()
        {
            InitializeComponent();
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
                CurrentPage = Locator.WorkOrderDetails,
                PageViewModel = _viewModel,
                IsModal = true
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!Settings.IsLoggedIn)
            {
                Application.Current.MainPage = new LoginPage() { BackgroundColor = Color.White };
                return;
            }
            else
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