using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Views
{
    public partial class SchedulerPage : ContentPage, IContentPage
    {
        SchedulerViewModel _viewModel;
        public SchedulerPage()
        {
            InitializeComponent();
            _viewModel = new SchedulerViewModel();
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
                CurrentPage = Locator.Scheduler,
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
                await _viewModel.InitializeAsync();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}