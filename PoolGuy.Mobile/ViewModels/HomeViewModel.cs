using System;
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
        }

        public ICommand OpenWebCommand { get; }
        public ICommand GoToCustomerCommand { get; }
    }
}