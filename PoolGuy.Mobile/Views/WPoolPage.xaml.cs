using CommonServiceLocator;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WPoolPage : ContentPage
    {
        CustomerViewModel _viewModel;
        public WPoolPage()
        {
            InitializeComponent();
            _viewModel = ServiceLocator.Current.GetInstance<CustomerViewModel>();
            _viewModel.SetView(this);
        }

        private void PoolType_OnTapped(object sender, MR.Gestures.TapEventArgs e)
        {
            poolTypePicker.Focus();
            poolTypePicker.SelectedIndexChanged += PoolTypePicker_SelectedIndexChanged;
            poolTypePicker.Unfocused += PoolTypePicker_Unfocused;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            poolTypePicker.SelectedIndexChanged -= PoolTypePicker_SelectedIndexChanged;
            poolTypePicker.Unfocused -= PoolTypePicker_Unfocused;
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
                    PoolModel_TypeError.IsVisible = false;
                    _viewModel.Pages.LastOrDefault().Pool.Type = (PoolType)Enum.Parse(typeof(PoolType), picker.SelectedItem.ToString());
                    _viewModel.Pages.LastOrDefault().NotifyPropertyChanged("Pool");

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }
    }
}