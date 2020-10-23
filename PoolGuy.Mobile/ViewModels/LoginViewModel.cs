using PoolGuy.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;

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
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            
        }
    }
}
