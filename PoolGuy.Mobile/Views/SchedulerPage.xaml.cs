using PoolGuy.Mobile.CustomControls;
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
            LongName.Unfocused += LongName_Unfocused;

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

        private void LongName_Unfocused(object sender, FocusEventArgs e)
        {
            if(sender is CustomEntry customEntry)
            {
                if(!string.IsNullOrEmpty(customEntry.Text) && string.IsNullOrEmpty(_viewModel.Scheduler.ShortName))
                {
                    _viewModel.Scheduler.ShortName = customEntry.Text.Substring(0, 3);
                    _viewModel.Scheduler.NotififyShortName();
                }
            }
        }

        protected override void OnDisappearing()
        {
            LongName.Unfocused -= LongName_Unfocused;
            base.OnDisappearing();
        }
    }
}