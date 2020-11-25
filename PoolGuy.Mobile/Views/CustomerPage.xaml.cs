using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using System;
using Xamarin.Forms;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.Views
{
    public partial class CustomerPage : ContentPage, IContentPage
    {
        CustomerViewModel _viewModel;
        public CustomerPage()
        {
            InitializeComponent();
            _viewModel = new CustomerViewModel();
            BindingContext = _viewModel;
        }

        private void PoolType_OnTapped(object sender, MR.Gestures.TapEventArgs e)
        {
            poolTypePicker.Focus();
            poolTypePicker.SelectedIndexChanged += PoolTypePicker_SelectedIndexChanged;
            poolTypePicker.Unfocused -= PoolTypePicker_Unfocused;
            poolTypePicker.Unfocused += PoolTypePicker_Unfocused;
        }

        private void PoolTypePicker_Unfocused(object sender, FocusEventArgs e)
        {
            poolTypePicker.SelectedIndexChanged -= PoolTypePicker_SelectedIndexChanged;
        }

        private void PoolTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Xamarin.Forms.Picker;

            if (picker != null && picker.SelectedItem != null)
            {
                try
                {
                    CustomerModel_Pool_TypeError.IsVisible = false;
                    _viewModel.Customer.Pool.Type = (PoolType)Enum.Parse(typeof(PoolType), picker.SelectedItem.ToString()); ;
                    _viewModel.OnPoolChanged();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
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
    }
}