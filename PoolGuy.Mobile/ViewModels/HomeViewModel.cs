using PoolGuy.Mobile.Extensions;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

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

        public async void Initialize()
        {
            if (MainThread.IsMainThread)
            {
                var device = await Utils.GetPositionAsync();
                if (device != null)
                {
                    Weather = await DependencyService.Get<IWeatherService>().GetWeather(device.Latitude, device.Longitude);
                }
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async() =>
                {
                    var device = await Utils.GetPositionAsync();
                    if (device != null)
                    {
                        Weather = await DependencyService.Get<IWeatherService>().GetWeather(device.Latitude, device.Longitude);
                    }
                });
            }
        }

        private WeatherRoot _weatherRoot;

        public WeatherRoot Weather
        {
            get { return _weatherRoot; }
            set { _weatherRoot = value; OnPropertyChanged("Weather"); }
        }

    }
}