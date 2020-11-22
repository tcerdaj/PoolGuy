using PoolGuy.Mobile.Extensions;
using PoolGuy.Mobile.Helpers;
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

        private void Initialize()
        {
            if (MainThread.IsMainThread)
            {

            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async() =>
                {
                    var device = await Utils.GetPositionAsync();
                    if (device != null)
                    {
                        var wather = await DependencyService.Get<IWeatherService>().GetWeather(device.Latitude, device.Longitude);

                    }
                });
            }
        }
    }
}