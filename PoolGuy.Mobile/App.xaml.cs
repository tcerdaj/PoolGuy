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
using PoolGuy.Mobile.Data.Controllers;

namespace PoolGuy.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            DependencyService.Register<ILocalDataStore<CustomerModel>, LocalDataStore<CustomerModel>>();
            DependencyService.Register<ILocalDataStore<AddressModel>, LocalDataStore<AddressModel>>();
            DependencyService.Register<ILocalDataStore<ContactModel>, LocalDataStore<ContactModel>>();
            DependencyService.Register<ILocalDataStore<PoolModel>, LocalDataStore<PoolModel>>();
            DependencyService.Register<ILocalDataStore<EquipmentModel>, LocalDataStore<EquipmentModel>>();
            DependencyService.Register<ILocalDataStore<EquipmentTypeModel>, LocalDataStore<EquipmentTypeModel>>();
            DependencyService.Register<ILocalDataStore<ManufactureModel>, LocalDataStore<ManufactureModel>>();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (!SimpleIoc.Default.IsRegistered<IUserDialogs>())
            {
                SimpleIoc.Default.Register<IUserDialogs>(() => new UserDialogs());
            }

            if (!SimpleIoc.Default.IsRegistered<CustomerViewModel>())
            {
                SimpleIoc.Default.Register<CustomerViewModel>(true);
            }

            if (!SimpleIoc.Default.IsRegistered<CustomerPageViewModel>())
            {
                SimpleIoc.Default.Register<CustomerPageViewModel>(true);
            }

            var nav = new NavigationService();
            nav.Configure(Locator.WizardCustomer, typeof(WizardCustomerPage));
            //nav.Configure("HomePage", typeof(HomePage));
            //nav.Configure("CustomerPage", typeof(CustomerPage));
            //nav.Configure("CarouselPage", typeof(Views.CarouselPage));

            if (!SimpleIoc.Default.IsRegistered<INavigationService>())
            {
                SimpleIoc.Default.Register<INavigationService>(() => nav);
            }

            CreateTables();
            MainPage = new AppShell();
           // MainPage.ToolbarItems.Add(new ToolbarItem { Text ="Test"});
        }

        private void CreateTables()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await new CustomerController().LocalData.CreateTableAsync();
                await new PoolController().LocalData.CreateTableAsync();
                await new AddressController().LocalData.CreateTableAsync();
                await new ContactInformationController().LocalData.CreateTableAsync();
                await new EquipmentController().LocalData.CreateTableAsync();
                await new EquipmentTypeController().LocalData.CreateTableAsync();
                await new ManufactureController().LocalData.CreateTableAsync();
            });
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