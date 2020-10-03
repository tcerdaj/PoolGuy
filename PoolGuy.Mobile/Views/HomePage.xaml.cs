﻿using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Views
{
    public partial class HomePage : ContentPage
    {
        HomeViewModel _viewModel;
        public HomePage()
        {
            InitializeComponent();
            _viewModel = new HomeViewModel() { IsBusy = false };
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.IsBusy = false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.IsBusy = true;
        }
    }
}