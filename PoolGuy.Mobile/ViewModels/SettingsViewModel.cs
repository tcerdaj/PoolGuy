using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.SampleData;
using PoolGuy.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using PoolGuy.Mobile.Extensions;
using System.Linq;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Data.SQLite;
using System.IO;

namespace PoolGuy.Mobile.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "").Replace("Search", "");
            OnPropertyChanged("DatabaseInfo");
        }

        public string[] DatabaseInfo
        {
            get { return GetDatabaseInfo(); }
        }
        
        private int _maxRow = 100;

        public int MaxRow
        {
            get { return _maxRow; }
            set { _maxRow = value; OnPropertyChanged("MaxRow"); }
        }

        public ICommand LogoutCommand
        {
            get { return new RelayCommand(async () => await LogoutAsync()); }
        }

        private async Task LogoutAsync()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                await NavigationService.NavigateToDialog(Locator.Login);
                Settings.IsLoggedIn = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand LoadSampleCustomersCommand
        {
            get { return new RelayCommand(async () => LoadSampleCustomersAsync()); }
        }

        private async void LoadSampleCustomersAsync()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                var customers = new CustomerListSample().Customers.Take(MaxRow);
                var customerController = new CustomerController();

                foreach (var c in customers)
                {
                    // Check if customer exist
                    var sourceCustomer = await customerController.LocalData.List(
                        new Data.Models.Query.SQLControllerListCriteriaModel
                        {
                            Filter = new List<Data.Models.Query.SQLControllerListFilterField>
                            {
                                new Data.Models.Query.SQLControllerListFilterField
                                {
                                    FieldName = "FirstName",
                                    ValueLBound = c.FirstName
                                },
                                new Data.Models.Query.SQLControllerListFilterField
                                {
                                    FieldName = "LastName",
                                    ValueLBound = c.LastName
                                }
                            }
                        });

                    if (!sourceCustomer.Any())
                    {
                        var index = customers.ToList().IndexOf(c);

                        // Parse class
                        CustomerModel customer = new CustomerModel
                        {
                            Address = new AddressModel
                            {
                                Address1 = c.Address1,
                                Address2 = c.Address2,
                                City = c.City,
                                State = c.State,
                                Zip = c.Zip
                            },
                            HomeAddress = new AddressModel 
                            {
                                Address1 = c.Address1,
                                Address2 = c.Address2,
                                City = c.City,
                                State = c.State,
                                Zip = c.Zip
                            },
                            Contact = new ContactModel
                            {
                                Phone = c.Phone,
                                CellPhone = c.CellPhone,
                                Email = c.Email
                            },
                            Pool = new PoolModel
                            {
                                Type = (Enums.PoolType)Enum.Parse(typeof(Enums.PoolType), c.Type, true),
                                Capacity = c.Capacity,
                                Surface = c.Surface
                            },
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            AdditionalInformation = c.AdditionalInformation,
                            ImageUrl = $"https://randomuser.me/api/portraits/men/{index}.jpg",
                            Index = index
                        };

                        await customerController.ModifyWithChildrenAsync(customer);
                    }
                }

                Message.Toast($"Customers successfully generated", TimeSpan.FromSeconds(3));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally { IsBusy = false; }
        }

        public ICommand NavigateToCommand
        {
            get
            {
                return new RelayCommand<Enums.ePage>(async (item) =>
                {
                    string page = item == Enums.ePage.Customer ? $"Search{item.ToString()}" : item.ToString();
                    await NavigationService.ReplaceRoot($"{page}Page");
                });
            }
        }

        public ICommand MangeCommand
        {
            get { return new RelayCommand<CustomerModel>(async (customer) => await Manage()); }
        }

        private Task Manage()
        {
            throw new NotImplementedException();
        }

        private string[] GetDatabaseInfo()
        {
            List<string> result = new List<string>();
            try
            {
                FileInfo file = new FileInfo (SQLiteControllerBase
                                       .DatabaseAsync
                                       .DatabasePath);

                var bytes = file.Length.BytesToString();
                result.Add($"Database version: {SQLiteControllerBase.DatabaseAsync.LibVersionNumber}");
                result.Add($"Database size: {bytes}");
                result.Add($"Database tables: {SQLiteControllerBase.DatabaseAsync.TableMappings.Count()}");

                return result.ToArray();
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);  
            }

            return new string[] { };
        }

        public ICommand AddStopItemsCommand
        {
            get { return new RelayCommand(() => AddStopItemsAsync()); }
        }

        private async void AddStopItemsAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
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
                    Message.Toast($"{items.Count} Items successfully generated", TimeSpan.FromSeconds(3));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally { IsBusy = false; }
        }
    }
}