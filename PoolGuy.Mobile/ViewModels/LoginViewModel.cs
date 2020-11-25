using Acr.UserDialogs;
using GalaSoft.MvvmLight.Ioc;
using PoolGuy.Mobile.Helpers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoolGuy.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            Title = this.GetType().Name.Replace("ViewModel", "");
        }

        private async void OnLoginClicked(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await Task.Delay(2000);
                await Shell.Current.GoToAsync(Locator.Home);
            }
            catch (Exception e)
            {
                if (Message != null)
                {
                    await Message.DisplayAlertAsync($"Unabble to navigate to {Locator.Home}", Title, "Ok");
                }
            }
            finally { IsBusy = false; }
        }
    }
}
