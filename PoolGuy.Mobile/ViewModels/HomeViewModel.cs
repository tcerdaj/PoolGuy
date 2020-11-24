using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Extensions;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Services.Interface;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace PoolGuy.Mobile.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
            Notify.RaiseNavigationAction(new Messages.RefreshMessage());
        }

        public ICommand OpenWebCommand { get; }
        public ICommand GoToCustomerCommand { get; }

        public async Task Initialize()
        {
            if (MainThread.IsMainThread)
            {
                await SetWeather()
                        .ConfigureAwait(false);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async() =>
                {
                    await SetWeather()
                            .ConfigureAwait(false);
                });
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
                               ValueLBound = DateTime.Now.Date.AddDays(-1).ToString(),
                               ValueUBound = DateTime.Now.Date.AddDays(1).ToString(),
                               CompareMethod = Data.Models.Query.SQLControllerListFilterField.CompareMethodEnum.GreaterThan 
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
                            WeatherId = weather.current.weather.FirstOrDefault().Id,
                            Icon = weather.current.weather.FirstOrDefault().Icon,
                            Temp = weather.current.temp,
                            DT = weather.current.dt,
                            Sunrise = weather.current.sunrise,
                            Sunset = weather.current.sunset,
                            FeelsLike = weather.current.feels_like,
                            Humidity = weather.current.humidity,
                            Description = weather.current.weather.FirstOrDefault().Description,
                            Clouds = weather.current.clouds,
                            Uvi = weather.current.uvi,
                            Rain = weather.daily.Sum(x => x.rain),
                            ThreeHoursRain = weather.daily.FirstOrDefault(x => string.IsNullOrEmpty(x.Rain?.ThreeHours)).Rain?.ThreeHours,
                            WindDeg = weather.current.wind_deg,
                            WindSpeed = weather.current.wind_speed
                        };

                        await new WeatherController().LocalData.Modify(Weather);
                        var w = await new WeatherController().LocalData.List().ConfigureAwait(false);
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