using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;
using System;

namespace PoolGuy.Mobile.Views
{
    public partial class SettingsPage : ContentPage, IContentPage
    {
        SettingsViewModel _viewModel;

        public SettingsPage()
        {
            try
            {
                InitializeComponent();
                _viewModel = new SettingsViewModel();
                BindingContext = _viewModel;
            }
            catch (Exception e)
            {
                throw;
            }
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
                CurrentPage = Locator.Settings,
                //PageViewModel = _viewModel,
                IsModal = true
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!Settings.IsLoggedIn)
            {
                Application.Current.MainPage = new LoginPage() { BackgroundColor = Color.White };
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            if (sender is Entry entry)
            {
                if (int.TryParse(entry.Text, out var rows) && rows > 100)
                {
                    _viewModel.MaxRow = 100;
                }
            }
        }
    }
}