﻿using GalaSoft.MvvmLight.Command;
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

                Message.Toast($"Customers successfully generated", TimeSpan.FromSeconds(10));
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
    }
}