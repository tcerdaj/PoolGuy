using PoolGuy.Mobile.ViewModels;
using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.Views
{
    public partial class CustomerPage : ContentPage
    {
        CustomerViewModel _viewModel;
        public CustomerPage()
        {
            InitializeComponent();
            _viewModel = new CustomerViewModel(this);
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
    }
}