using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        private static HomeViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();

            _viewModel = ServiceLocator.Current.GetInstance<HomeViewModel>();
            
            BindingContext = _viewModel;
            this.IsPresented = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //var navPage = ((MasterDetailPage)App.Current.MainPage).Detail as Page;
        }
    }
}