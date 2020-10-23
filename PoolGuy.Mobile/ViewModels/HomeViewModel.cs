using PoolGuy.Mobile.Helpers;
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
    }
}