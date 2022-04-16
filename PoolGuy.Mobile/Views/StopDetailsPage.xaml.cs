using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Views
{
    public partial class StopDetailsPage : ContentPage, IContentPage
    {
        StopDetailsViewModel _viewModel;
        public StopDetailsPage()
        {
            InitializeComponent();
            _viewModel = new StopDetailsViewModel(new MobileCustomerModel());
            BindingContext = _viewModel;
        }

        public StopDetailsPage(MobileCustomerModel customer)
        {
            InitializeComponent();
            _viewModel = new StopDetailsViewModel(customer);
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
                CurrentPage = Locator.StopDetails,
                PageViewModel = _viewModel,
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

            await _viewModel.InitializeAsync();
            _viewModel.IsBusy = false;
        }

        private void Notes_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if(e.NewTextValue != e.OldTextValue)
            {
                _viewModel.IsEditing = true;
            }
        }
    }
}