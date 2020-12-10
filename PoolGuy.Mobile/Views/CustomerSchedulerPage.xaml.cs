using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace PoolGuy.Mobile.Views
{
    public partial class CustomerSchedulerPage : ContentPage, IContentPage
    {
        CustomerSchedulerViewModel _viewModel;

        public CustomerSchedulerPage(Guid selectedSchedulerId)
        {
            try
            {
                InitializeComponent();
                _viewModel = new CustomerSchedulerViewModel() { 
                    SelectedSchedulerId = selectedSchedulerId
                };
                BindingContext = _viewModel;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public CustomerSchedulerPage(List<SchedulerModel> list)
        {
            InitializeComponent();
            _viewModel = new CustomerSchedulerViewModel();
            _viewModel.Schedulers = new System.Collections
                .ObjectModel
                .ObservableCollection<SchedulerModel>(list);
            BindingContext = _viewModel;
        }

        public CustomerSchedulerPage(CustomerModel customer)
        {
            InitializeComponent();
            _viewModel = new CustomerSchedulerViewModel();
            _viewModel.Customers = new System.Collections
                .ObjectModel
                .ObservableCollection<CustomerModel>(new List<CustomerModel> { customer });
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
                await _viewModel.InitSchedulers();
                await _viewModel.InitializeAsync();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == string.Empty)
            {
                // Yes, which one?
                if (e.OldTextValue == null)
                {
                    Debug.WriteLine("Initialize");
                }
                else if (e.OldTextValue.Length > 1)
                {
                    // Cancel has most probably been pressed
                    Debug.WriteLine("Cancel Pressed");
                }
                else
                {
                    // Backspace pressed on single character
                    // Cancel pressed on single character
                    Debug.WriteLine("Backspace or Cancel Pressed");
                }
            }
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            _viewModel.RefreshList();
        }

        private void SelectAllck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox check)
            {
                _viewModel.SelectCommand.Execute(check.IsChecked);
            }
        }
    }
}