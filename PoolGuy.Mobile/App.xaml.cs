using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PoolGuy.Mobile.Services;
using PoolGuy.Mobile.Views;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Data.SQLite;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.ViewModels;

namespace PoolGuy.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            DependencyService.Register<ILocalDataStore<CustomerModel>, LocalDataStore<CustomerModel>>();

            DependencyService.Register<ILocalDataStore<PoolModel>, LocalDataStore<PoolModel>>();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (!SimpleIoc.Default.IsRegistered<IUserDialogs>())
            {
                SimpleIoc.Default.Register<IUserDialogs>(() => new UserDialogs());
            }

            if (!SimpleIoc.Default.IsRegistered<CarouselViewModel>())
            {
                SimpleIoc.Default.Register<CarouselViewModel>(true);
            }

            //var nav = new NavigationService();
            //nav.Configure(Locator.Popup.ActionSheetPopup, typeof(ActionSheetPopupPage));
            //nav.Configure("CarouselPage", typeof(Views.CarouselPage));
            //nav.Configure("HomePage", typeof(HomePage));
            //nav.Configure("CustomerPage", typeof(CustomerPage));
            //nav.Configure("CarouselPage", typeof(Views.CarouselPage));

            //if (!SimpleIoc.Default.IsRegistered<INavigationService>())
            //{
            //    SimpleIoc.Default.Register<INavigationService>(() => nav);
            //}

            MainPage = new AppShell();
           // MainPage.ToolbarItems.Add(new ToolbarItem { Text ="Test"});
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }

    
}