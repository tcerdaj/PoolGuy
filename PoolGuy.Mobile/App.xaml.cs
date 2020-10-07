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

            var nav = new NavigationService();
            nav.Configure(Locator.Popup.ActionSheetPopup, typeof(ActionSheetPopupPage));

            if (!SimpleIoc.Default.IsRegistered<INavigationService>())
            {
                SimpleIoc.Default.Register<INavigationService>(() => nav);
            }
            
            MainPage = new AppShell();
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