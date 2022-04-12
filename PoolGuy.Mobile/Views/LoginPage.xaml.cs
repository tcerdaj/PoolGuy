using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage, IContentPage
    {
        LoginViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();
            _viewModel = new LoginViewModel();
            this.BindingContext = _viewModel;
        }

        public void CleanUp()
        {
        }

        public void Initialize()
        {
        }

        public MobileNavigationModel OnSleep()
        {
            return new MobileNavigationModel
            {
                CurrentPage = Locator.Login,
                //PageViewModel = _viewModel,
                IsModal = true
            };
        }
    }
}