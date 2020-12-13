using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Views;
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
            Settings.IsLoggedIn = false;
        }

        private async void OnLoginClicked(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            Settings.IsLoggingIn = true;

            try
            {
                await Task.Delay(2000);

                await NavigationService.ReplaceRoot(Locator.Home);
                await NavigationService.CloseModal();
                Notify.RaiseHomeAction(new Messages.RefreshMessage());
                Settings.IsLoggedIn = true;
            }
            catch (Exception e)
            {
                if (Message != null)
                {
                    await Message.DisplayAlertAsync($"Unabble to navigate to {Locator.Home}", Title, "Ok");
                }
            }
            finally { 
                IsBusy = false;
                Settings.IsLoggingIn = false;
            }
        }
    }
}
