using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models.Weather;
using PoolGuy.Mobile.Extensions;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Services.Interface;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using PoolGuy.Mobile.Data.Helpers;

namespace PoolGuy.Mobile.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
            Notify.RaiseNavigationAction(new Messages.RefreshMessage());
            SubscribeMessages();
        }

        private void SubscribeMessages()
        {
            Notify.SubscribeHomeAction(async(sender) =>
            {
                await Initialize();
            });
        }

        public ICommand OpenWebCommand { get; }
        public ICommand GoToCustomerDetailsCommand { get; }
        public ICommand GoToSearchCustomerCommand 
        { 
            get 
            {
                return new RelayCommand(async () =>
                {
                    await Shell.Current.GoToAsync(Locator.SearchCustomer);
                });
            } 
        }

        public async Task Initialize()
        {
            if(IsBusy || Weather != null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                if (!Settings.IsLoggedIn)
                {
                    await Shell.Current.GoToAsync(Locator.Login);
                    return;
                }

                if (MainThread.IsMainThread)
                {
                    await SetWeather()
                            .ConfigureAwait(false);
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await SetWeather()
                                .ConfigureAwait(false);
                    });
                }
            }
            catch (Exception e)
            {
                await Message.DisplayAlertAsync(e.Message, "Initialize", "Ok");
            }
            finally 
            {
                IsBusy = false;
            }
        }

        private async Task SetWeather()
        {
            var storagedWeather = await new WeatherController().LocalData.List(new Data.Models.Query.SQLControllerListCriteriaModel
            {
                Filter = new System.Collections.Generic.List<Data.Models.Query.SQLControllerListFilterField>
                       {
                           new Data.Models.Query.SQLControllerListFilterField
                           {
                               FieldName = "Created",
                               ValueLBound = DateTime.Now.Date.ToString(),
                               ValueUBound = DateTime.Now.Date.AddDays(1).AddTicks(-1).ToString(),
                               DateKind = Data.Models.Query.SQLControllerListFilterField.DateKindEnum.Localized
                           }
                       }
            }).ConfigureAwait(false);

            if (storagedWeather.Any())
            {
                Weather = storagedWeather.FirstOrDefault();
            }
            else
            {
                var device = await Utils.GetPositionAsync();
                if (device != null)
                {
                    var weather = await DependencyService.Get<IWeatherService>().GetOneCall(device.Latitude, device.Longitude);
                    if (weather != null)
                    {
                        Weather = new WeatherModel
                        {
                            WeatherJson = JsonConvert.SerializeObject(weather)
                        };

                        await new WeatherController().LocalData.Modify(Weather);
                        Weather.RaiseFields();
                    }
                }
            }
        }

        private WeatherModel _weatherRoot;

        public WeatherModel Weather
        {
            get { return _weatherRoot; }
            set { _weatherRoot = value; OnPropertyChanged("Weather"); }
        }
    }
}