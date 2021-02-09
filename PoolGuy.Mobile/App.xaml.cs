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
using PoolGuy.Mobile.Controllers;
using System;
using System.Diagnostics;
using SQLite;
using PoolGuy.Mobile.Data.Models.GoogleMap;
using System.Collections.Generic;

namespace PoolGuy.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            RegisterTables();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (!SimpleIoc.Default.IsRegistered<IUserDialogs>())
            {
                SimpleIoc.Default.Register<IUserDialogs>(() => new UserDialogs());
            }

            if (!SimpleIoc.Default.IsRegistered<HomeViewModel>())
            {
                SimpleIoc.Default.Register<HomeViewModel>(true);
            }

            if (!SimpleIoc.Default.IsRegistered<CustomerPageViewModel>())
            {
                SimpleIoc.Default.Register<CustomerPageViewModel>(true);
            }

            var nav = new NavigationService();
            nav.Configure(Locator.WizardCustomer, typeof(WizardCustomerPage));
            nav.Configure(Locator.Home, typeof(HomePage));
            nav.Configure(Locator.Popup.ActionSheetPopup, typeof(ActionSheetPopupPage));
            nav.Configure(Locator.Login, typeof(LoginPage));
            nav.Configure(Locator.Equipment, typeof(EquipmentPage));
            nav.Configure(Locator.Customer, typeof(CustomerPage));
            nav.Configure(Locator.Settings, typeof(SettingsPage));
            nav.Configure(Locator.Customer, typeof(CustomerPage));
            nav.Configure(Locator.Scheduler, typeof(SchedulerPage));
            nav.Configure(Locator.CustomerScheduler, typeof(CustomerSchedulerPage));
            nav.Configure(Locator.Map, typeof(MapPage));
            nav.Configure(Locator.Stops, typeof(StopsPage));
            nav.Configure(Locator.StopDetails, typeof(StopDetailsPage));

            if (!SimpleIoc.Default.IsRegistered<INavigationService>())
            {
                SimpleIoc.Default.Register<INavigationService>(() => nav);
            }

            if(Settings.IsLoggedIn)
            {
                MainPage = new MainPage();
                Notify.RaiseHomeAction(new Messages.RefreshMessage());
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
                DependencyService.Register<ILocalDataStore<WorkOrderItemModel>, LocalDataStore<WorkOrderItemModel>>();
                DependencyService.Register<ILocalDataStore<StopItemModel>, LocalDataStore<StopItemModel>>();
                DependencyService.Register<ILocalDataStore<StopModel>, LocalDataStore<StopModel>>();
                DependencyService.Register<ILocalDataStore<ReportModel>, LocalDataStore<ReportModel>>();
                DependencyService.Register<ILocalDataStore<EquipmentIssueModel>, LocalDataStore<EquipmentIssueModel>>();
                DependencyService.Register<ILocalDataStore<DirectionHistory>, LocalDataStore<DirectionHistory>>();
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
                await CreateLinkTables();
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
                await new WorkOrderItemController().LocalData.CreateTableAsync();
                await new StopController().LocalData.CreateTableAsync();
                await new StopItemController().LocalData.CreateTableAsync();
                await new ItemController().LocalData.CreateTableAsync();
                await new ReportController().LocalData.CreateTableAsync();
                await new EquipmentIssueController().LocalData.CreateTableAsync();
                await new ImageController().LocalData.CreateTableAsync();
                await new DirectionController().LocalData.CreateTableAsync();
            }
            catch (System.Exception e)
            {
                throw;
            }
        }

        private static async Task CreateLinkTables()
        {
            if (SQLiteControllerBase.DatabaseAsync.TableMappings.All(m => m.MappedType.Name != typeof(CustomerSchedulerModel).Name))
            {
                await SQLiteControllerBase.DatabaseAsync.CreateTableAsync(typeof(CustomerSchedulerModel), CreateFlags.None);
            }

            if (SQLiteControllerBase.DatabaseAsync.TableMappings.All(m => m.MappedType.Name != typeof(UserRoleModel).Name))
            {
                await SQLiteControllerBase.DatabaseAsync.CreateTableAsync(typeof(UserRoleModel), CreateFlags.None);
            }
        }

        private async Task ClearTablesAsync()
        {
            try
            {

                await SQLiteControllerBase.DatabaseAsync.DropTableAsync<CustomerSchedulerModel>();
                await SQLiteControllerBase.DatabaseAsync.CreateTableAsync<CustomerSchedulerModel>();
                await new SchedulerController().LocalData.ClearTableAsync();
                await new CustomerController().LocalData.ClearTableAsync();
                await new PoolController().LocalData.ClearTableAsync();
                await new AddressController().LocalData.ClearTableAsync();
                await new ContactInformationController().LocalData.ClearTableAsync();
                await new EquipmentController().LocalData.ClearTableAsync();
                await new EquipmentTypeController().LocalData.ClearTableAsync();
                await new ManufactureController().LocalData.ClearTableAsync();
                await new WeatherController().LocalData.ClearTableAsync();
                await new UserController().LocalData.ClearTableAsync();
                await new RoleController().LocalData.ClearTableAsync();
                await new WorkOrderController().LocalData.ClearTableAsync();
                await new WorkOrderItemController().LocalData.ClearTableAsync();
                await new StopController().LocalData.ClearTableAsync();
                await new ItemController().LocalData.ClearTableAsync();
                await new ReportController().LocalData.ClearTableAsync();
                await new EquipmentIssueController().LocalData.ClearTableAsync();
                await new ImageController().LocalData.ClearTableAsync();
                await new DirectionController().LocalData.ClearTableAsync();
            }
            catch (System.Exception e)
            {
                throw;
            }
        }

        protected override async void OnStart()
        {
            var result = await DependencyService.Get<IPermissionService>()
                .CheckPermissions(Permission.Storage);

            if (!result.Any(x => x.Value == PermissionStatus.Granted))
            {
                return;
            }

            await AddDefaultValues();

            //await ClearTablesAsync();
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

            base.OnStart();
        }

        private static async Task AddDefaultValues()
        {
            var stopItemController = new StopItemController().LocalData;
            var stopItems = await stopItemController.List(new Data.Models.Query.SQLControllerListCriteriaModel
            {
                Filter = new System.Collections.Generic.List<Data.Models.Query.SQLControllerListFilterField> {
               new Data.Models.Query.SQLControllerListFilterField {
                    FieldName = "ItemType",
                    ValueLBound = ((int)Enums.eItemType.Stop).ToString()
               }}
            });

            if (!stopItems.Any())
            {
                var items = new List<StopItemModel> {
                 new StopItemModel {
                   Index = 0,
                   Name = "CHL",
                   Description ="Chlorine is a chemical element with the symbol Cl and atomic number 17. The second-lightest of the halogens, it appears between fluorine and bromine in the periodic table and its properties are mostly intermediate between them",
                   ImageUrl = "https://www.istockphoto.com/photo/swimming-pool-water-treatment-with-chlorine-tablets-gm182174385-10189979",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Gallon,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 69.99m,
                 },
                 new StopItemModel {
                   Index = 1,
                   Name = "pH",
                   Description ="pH is a measure of hydrogen ion concentration, a measure of the acidity or alkalinity of a solution. The pH scale usually ranges from 0 to 14. Aqueous solutions at 25°C with a pH less than 7 are acidic, while those with a pH greater than 7 are basic or alkaline. A pH level of 7.0 at 25°C is defined as neutral because the concentration of H3O+ equals the concentration of OH− in pure water. Very strong acids might have a negative pH, while very strong bases might have a pH greater than 14.",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.VPM,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 0m,
                 },
                 new StopItemModel {
                   Index = 2,
                   Name = "ALK",
                   Description ="Alkalinity The buffering capacity of a water body; a measure of the ability of the water body to neutralize acids and bases and thus maintain a fairly stable pH leve",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.VPM,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 0m,
                 },
                  new StopItemModel {
                   Index = 3,
                   Name = "CAL",
                   Description ="Calcium contribute to the physiology and biochemistry of organisms cell",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.VPM,
                   Frequency = 3,
                   FrequencyType = Enums.eFrequencyType.Monthly,
                   Price = 0m,
                 },
                  new StopItemModel {
                   Index = 4,
                   Name = "CY",
                   Description ="Cyanuric Acid One of the most common chemicals used for the prevention of chlorine loss in swimming pools is Cyanuric Acid (also called CYA, Conditioner, or Stabilizer) it protects the free chlorine from being destroyed by the sun ultraviolet rays",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.VPM,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Monthly,
                   Price = 0m,
                 },
                 new StopItemModel {
                   Index = 5,
                   Name = "WT",
                   Description ="Water Temperature",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Temperature,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 0m,
                 },
                 new StopItemModel {
                   Index = 6,
                   Name = "L-CHLO",
                   Description ="Liquid bleach",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Gallon,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 20m,
                 },
                 new StopItemModel {
                   Index = 7,
                   Name = "Tbts",
                   Description ="Tablets Triclor",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Unit,
                   Price = 20m,
                 },
                 new StopItemModel {
                   Index = 8,
                   Name = "Skim",
                   Description ="Surface cleaner",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Device,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 20m,
                 },
                 new StopItemModel {
                   Index = 9,
                   Name = "Pump",
                   Description ="Water pump",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Device,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                 },
                 new StopItemModel {
                   Index = 10,
                   Name = "Filter",
                   Description ="Pool filter",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Device,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Monthly,
                 },
                 new StopItemModel {
                   Index = 11,
                   Name = "H2o",
                   Description ="Water check",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Gallon,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Monthly,
                 },
                 new StopItemModel {
                   Index = 12,
                   Name = "Vac-p",
                   Description ="Vacuum pool",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Device,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                 },
                 new StopItemModel {
                   Index = 13,
                   Name = "#S-Alk",
                   Description ="Scoop",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Device,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                 }
                };

                await stopItemController.InsertAll(items);
            }
        }

        protected override void OnSleep()
        {
            try
            {
                if (((MasterDetailPage)Application.Current.MainPage).Detail is NavigationPage navigation)
                {
                    AppStateController.SaveViewState(navigation.Navigation.ModalStack.Any()
                        ? ((IContentPage)navigation.Navigation.ModalStack.Last()).OnSleep()
                        : ((IContentPage)navigation.Navigation.NavigationStack.Last()).OnSleep());


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