using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
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
        private static CustomerViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();

            viewModel = ServiceLocator.Current.GetInstance<CustomerViewModel>();

            BindingContext = viewModel;

            this.IsPresented = false;
        }

        protected override void OnAppearingAsync()
        {
            base.OnAppearing();
            //var navPage = ((MasterDetailPage)App.Current.MainPage).Detail as Page;
        }
    }
}