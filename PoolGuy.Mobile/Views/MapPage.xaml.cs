using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.Data.Models;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using PoolGuy.Mobile.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms.GoogleMaps;

namespace PoolGuy.Mobile.Views
{
    public partial class MapPage : ContentPage, IContentPage
    {
        MapViewModel _viewModel;

        public MapPage(List<CustomerModel> model)
        {
            try
            {
                InitializeComponent();
                _viewModel = new MapViewModel() { Customers = model};
                BindingContext = _viewModel;

                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Globals.BranchLocation.Latitude,
                           Globals.BranchLocation.Longitude), Distance.FromMiles(2)));
                map.MapType = MapType.Street;
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
                CurrentPage = Locator.Map,
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
         }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}