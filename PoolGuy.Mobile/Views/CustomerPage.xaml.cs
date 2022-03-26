using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerPage : ContentPage, IContentPage
    {
        CustomerViewModel _viewModel;
        Color _primaryColor, _unselectedColor;
        public CustomerPage()
        {
            try
            {
                InitializeComponent();
                _viewModel = new CustomerViewModel();
                _viewModel.Init();
                BindingContext = _viewModel;
                
                _primaryColor = (Color)Application.Current.Resources["Primary"];
                _unselectedColor = (Color)Application.Current.Resources["UnselectedColor"];
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public CustomerPage(CustomerModel customer)
        {
            try
            {
                InitializeComponent();
                _viewModel = new CustomerViewModel();
                _viewModel.Init(customer);
                BindingContext = _viewModel;
                _primaryColor = (Color)Application.Current.Resources["Primary"];
                _unselectedColor = (Color)Application.Current.Resources["UnselectedColor"];
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.IsBusy = false;
            styleSwitch.Toggled += StyleSwitch_Toggled;
            _viewModel.Page = this;
        }

        private void StyleSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            _viewModel.Customer.Active = e.Value;
            _viewModel.IsEditing = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            poolTypePicker.SelectedIndexChanged -= PoolTypePicker_SelectedIndexChanged;
            poolTypePicker.Unfocused -= PoolTypePicker_Unfocused;
        }

        public void Initialize()
        {
        }

        public MobileNavigationModel OnSleep()
        {
            return new MobileNavigationModel
            {
                CurrentPage = Locator.Customer,
                PageViewModel = _viewModel,
                IsModal = true
            };
        }

        public void CleanUp()
        {
            
        }

        private void PoolType_OnTapped(object sender, MR.Gestures.TapEventArgs e)
        {
            poolTypePicker.Focus();
            poolTypePicker.SelectedIndexChanged += PoolTypePicker_SelectedIndexChanged;
            poolTypePicker.Unfocused += PoolTypePicker_Unfocused;
        }

        private void PoolTypePicker_Unfocused(object sender, FocusEventArgs e)
        {
            poolTypePicker.SelectedIndexChanged -= PoolTypePicker_SelectedIndexChanged;
        }

        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.OldTextValue != null && e.NewTextValue != e.OldTextValue && _viewModel.InitCompleted)
            {
                _viewModel.IsEditing = true;
            }
        }

        private void Scheduler_On_Day_Checked(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (checkBox.BindingContext is SchedulerModel scheduler && _viewModel.Customer?.Scheduler != null)
                {
                    if (e.Value)
                    {
                        _viewModel.Customer.Scheduler.Add(scheduler);
                    }
                    else
                    {
                        var index = _viewModel.Customer.Scheduler.FindIndex(x => x.Id == scheduler.Id);
                        if (index > -1)
                        {
                            _viewModel.Customer.Scheduler.RemoveAt(index);
                        }
                    }

                    _viewModel.IsEditing = true;
                }
            }
        }

        private void PoolTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is Xamarin.Forms.Picker picker)
            {
                try
                {
                    if (picker.SelectedItem is PoolType poolType)
                    {
                        _viewModel.IsEditing = true;
                        _viewModel.Customer.Pool.Type = poolType;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }
    }
}