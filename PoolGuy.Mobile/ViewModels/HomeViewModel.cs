using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models.Weather;
using PoolGuy.Mobile.Extensions;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Services.Interface;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Microcharts;
using SkiaSharp;
using System.Diagnostics;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.Config;
using System.Xml.Serialization;

namespace PoolGuy.Mobile.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            if (Globals.CurrentPage != Enums.ePage.Home)
            {
                Globals.CurrentPage = Enums.ePage.Home;
            }
            
            Title = this.GetType().Name.Replace("ViewModel", "");
            Notify.RaiseNavigationAction(new Messages.RefreshMessage());
            SubscribeMessages();
        }

        private bool _initialized;

        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        private Chart _temperature;
        [XmlIgnore]
        public Chart Temperature
        {
            get { return _temperature; }
            set { _temperature = value; OnPropertyChanged("Temperature"); }
        }

        public string TempColor
        {
            get
            {
                if(Weather == null)
                {
                    return "#7B7575";
                }

                return GetTemperatureColor(Weather.Main.current.temp);
            }
        }

        private Chart _rain;
        [XmlIgnore]
        public Chart Rain
        {
            get { return _rain; }
            set { _rain = value; OnPropertyChanged("Rain"); }
        }

        private void SubscribeMessages()
        {
            Notify.SubscribeHomeAction(async(sender) =>
            {
                if (Globals.CurrentPage != Enums.ePage.Home)
                {
                    Globals.CurrentPage = Enums.ePage.Home;
                }

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
                    await NavigationService.NavigateToDialog(Locator.Customer);
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
               await SetWeather();
            }
            catch (Exception e)
            {
                await Message.DisplayAlertAsync(e.Message, "Initialize", "Ok");
            }
            finally 
            {
                IsBusy = false;
                Initialized = true;
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
            });

            if (storagedWeather.Any())
            {
                Weather = storagedWeather.FirstOrDefault();
            }
            else
            {
                var device = await Utils.GetPositionAsync();
                if (device != null)
                {
                    WeatherHistoryRoot weather = null;
                    try
                    {
                        weather = await DependencyService.Get<IWeatherService>()
                            .GetOneCall(device.Latitude, 
                            device.Longitude, 
                            Config.ApiKeys.FirstOrDefault(x=>x.ConsumerKey =="OpenWeather").ConsumerSecret);
                    }
                    catch (Exception e)
                    {
                        Weather = (await new WeatherController().LocalData.List().ConfigureAwait(false)).LastOrDefault();
                    }

                    if (weather != null && Weather == null)
                    {
                        Weather = new WeatherModel
                        {
                            Id = Guid.Empty,
                            WeatherJson = JsonConvert.SerializeObject(weather)
                        };

                        await new WeatherController().LocalData.Modify(Weather);
                        Weather.RaiseFields();
                    }
                }
            }

            if (Weather != null)
            {
                Temperature = new LineChart
                {
                    Entries = Weather.Main.daily.Select(x => new ChartEntry(x.temp.day)
                    {
                        Label = CastDtToDayOfWeek(x.dt),
                        ValueLabel = x.temp.day.ToString("f0"),
                        Color = SKColor.Parse(GetTemperatureColor(x.temp.day))
                    })
                };

                Rain = new LineChart
                {
                    Entries = Weather.Main.daily.Select(x => new ChartEntry(x.rain)
                    {
                        Label = CastDtToDayOfWeek(x.dt),
                        ValueLabel = x.rain.ToString("f0"),
                        Color = SKColor.Parse("#2389da")
                    })
                };

                OnPropertyChanged("TempColor");
            }
        }

        private WeatherModel _weatherRoot;
        [XmlIgnore]
        public WeatherModel Weather
        {
            get { return _weatherRoot; }
            set { _weatherRoot = value; OnPropertyChanged("Weather"); }
        }

        string GetTemperatureColor(float color)
        {
            string result = "";
          
            switch (color)
            {
                case float n when(n > 159):
                    result = "#FF00E0";
                        break;
                case float n when (n > 145):
                    result = "#FF0070";
                    break;
                case float n when (n > 129):
                    result = "#FF0000";
                    break;
                case float n when (n > 121):
                    result = "#FF3200";
                    break;
                case float n when (n > 113):
                    result = "#FF5a00";
                    break;
                case float n when (n > 101):
                    result = "#FF9600";
                    break;
                case float n when (n > 91):
                    result = "#FFc800";
                    break;
                case float n when (n > 83):
                    result = "#FFf000";
                    break;
                case float n when (n > 79):
                    result = "#fdff00";
                    break;
                case float n when (n > 77):
                    result = "#d7ff00";
                    break;
                case float n when (n > 67):
                    result = "#17ff00";
                    break;
                case float n when (n > 59):
                    result = "#00ff83";
                    break;
                case float n when (n > 55):
                    result = "#00ffd0";
                    break;
                case float n when (n > 53):
                    result = "#00fff4";
                    break;
                case float n when (n > 49):
                    result = "#00d4ff";
                    break;
                case float n when (n > 39):
                    result = "#0094ff";
                    break;
                case float n when (n > 31):
                    result = "#0044ff";
                    break;
                case float n when (n > 23):
                    result = "#0002ff";
                    break;
                case float n when (n > 0):
                    result = "#0500ff";
                    break;
                default:
                    result = "#0500ff";
                    break;
            }

            return result;
        }

        string CastDtToDayOfWeek(int dt)
        {
            try
            {
                var culture = System.Globalization.CultureInfo.CurrentCulture;
                
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(dt).ToLocalTime();

                return culture.DateTimeFormat.GetAbbreviatedDayName(dtDateTime.DayOfWeek);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return "";
            }
        }

        public ICommand NavigateToCommand
        {
            get
            {
                return new RelayCommand<Enums.ePage>(async (item) =>
                {
                    string page = item.ToString();
                    await NavigationService.ReplaceRoot($"{page}Page");
                });
            }
        }
    }
}