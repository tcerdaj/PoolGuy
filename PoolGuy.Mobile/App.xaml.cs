using Xamarin.Forms;
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
using Plugin.Permissions.Abstractions;
using PoolGuy.Mobile.Data.Models.Weather;
using System.Threading.Tasks;
using PoolGuy.Mobile.Data.Helpers;
using System.Linq;
using Xamarin.Essentials;
using PoolGuy.Mobile.Controllers;
using System;
using System.Diagnostics;

namespace PoolGuy.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            //Device.SetFlags(new string[] { "RadioButton_Experimental" });

            RegisterTables();

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
            //nav.Configure(Locator.WizardCustomer, typeof(WizardCustomerPage));
            //nav.Configure(Locator.Home, typeof(HomePage));
            //nav.Configure(Locator.SearchCustomer, typeof(SearchCustomerPage));
            //nav.Configure(Locator.Login, typeof(LoginPage));
            //nav.Configure(Locator.Equipment, typeof(EquipmentPage));

            if (!SimpleIoc.Default.IsRegistered<INavigationService>())
            {
                SimpleIoc.Default.Register<INavigationService>(() => nav);
            }

            if(Settings.IsLoggedIn)
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        private void RegisterTables()
        {
            try
            {
                Settings.TabletsRegistered = null;
                
                DependencyService.Register<MockDataStore>();
                DependencyService.Register<ILocalDataStore<CustomerSchedulerModel>, LocalDataStore<CustomerSchedulerModel>>();
                DependencyService.Register<ILocalDataStore<SchedulerModel>, LocalDataStore<SchedulerModel>>();
                DependencyService.Register<ILocalDataStore<CustomerModel>, LocalDataStore<CustomerModel>>();
                DependencyService.Register<ILocalDataStore<AddressModel>, LocalDataStore<AddressModel>>();
                DependencyService.Register<ILocalDataStore<ContactModel>, LocalDataStore<ContactModel>>();
                DependencyService.Register<ILocalDataStore<EquipmentModel>, LocalDataStore<EquipmentModel>>();
                DependencyService.Register<ILocalDataStore<EquipmentTypeModel>, LocalDataStore<EquipmentTypeModel>>();
                DependencyService.Register<ILocalDataStore<ManufactureModel>, LocalDataStore<ManufactureModel>>();
                DependencyService.Register<ILocalDataStore<PoolModel>, LocalDataStore<PoolModel>>();
                DependencyService.Register<ILocalDataStore<WeatherModel>, LocalDataStore<WeatherModel>>();
                DependencyService.Register<ILocalDataStore<EntityImageModel>, LocalDataStore<EntityImageModel>>();
                DependencyService.Register<ILocalDataStore<UserModel>, LocalDataStore<UserModel>>();
                DependencyService.Register<ILocalDataStore<RoleModel>, LocalDataStore<RoleModel>>();
                DependencyService.Register<ILocalDataStore<WorkOrderModel>, LocalDataStore<WorkOrderModel>>();
                DependencyService.Register<ILocalDataStore<WorkOrderDetailsModel>, LocalDataStore<WorkOrderDetailsModel>>();
                DependencyService.Register<ILocalDataStore<ItemModel>, LocalDataStore<ItemModel>>();
                DependencyService.Register<ILocalDataStore<ReportModel>, LocalDataStore<ReportModel>>();
                DependencyService.Register<ILocalDataStore<EquipmentIssueModel>, LocalDataStore<EquipmentIssueModel>>();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task CreateTablesAsync()
        {
            try
            {
                await new CustomerSchedulerController().LocalData.CreateTableAsync();
                await new SchedulerController().LocalData.CreateTableAsync();
                await new CustomerController().LocalData.CreateTableAsync();
                await new PoolController().LocalData.CreateTableAsync();
                await new AddressController().LocalData.CreateTableAsync();
                await new ContactInformationController().LocalData.CreateTableAsync();
                await new EquipmentController().LocalData.CreateTableAsync();
                await new EquipmentTypeController().LocalData.CreateTableAsync();
                await new ManufactureController().LocalData.CreateTableAsync();
                await new WeatherController().LocalData.CreateTableAsync();
                await new UserController().LocalData.CreateTableAsync();
                await new RoleController().LocalData.CreateTableAsync();
                await new WorkOrderController().LocalData.CreateTableAsync();
                await new WorkOrderDetailsController().LocalData.CreateTableAsync();
                await new ItemController().LocalData.CreateTableAsync();
                await new ReportController().LocalData.CreateTableAsync();
                await new EquipmentIssueController().LocalData.CreateTableAsync();
                await new ImageController().LocalData.CreateTableAsync();
            }
            catch (System.Exception e)
            {
                throw;
            }
        }

        protected override async void OnStart()
        {
            base.OnStart();

            var result = await DependencyService.Get<IPermissionService>()
                    .CheckPermissions(Permission.Storage);

            if (!result.Any(x => x.Value == Plugin.Permissions.Abstractions.PermissionStatus.Granted))
            {
                return;
            }

            await CreateTablesAsync();
            
            var navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
            var navList = AppStateController.RestoreState();
            AppStateController.ClearNavigationMetaStack();

            foreach (var nav in navList)
            {
                switch (nav.CurrentPage)
                {
                    case "MainPage":
                        await navigationService.ReplaceRoot(Locator.Home);
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void OnSleep()
        {
            try
            {
                
                if (Shell.Current != null && Shell.Current.Navigation.NavigationStack.Any() || Shell.Current.Navigation.ModalStack.Any())
                {

                    if (Shell.Current.Navigation.ModalStack.Any())
                    {
                        AppStateController.SaveViewState(((IContentPage)Shell.Current.Navigation.ModalStack.Last()).OnSleep());
                    }

                    if (Shell.Current.Navigation.NavigationStack.Any())
                    {

                        var page = (IContentPage)Shell.Current.Navigation.NavigationStack.Last();
                        AppStateController.SaveViewState(((IContentPage)Shell.Current.Navigation.NavigationStack.Last()).OnSleep());
                    }

                    AppStateController.SaveFinalState();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        protected override void OnResume()
        {
            try
            {
                AppStateController.ResetState();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}