using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchCustomerPage : ContentPage, IContentPage
    {
        SearchCustomerViewModel _viewModel;
        public SearchCustomerPage()
        {
            InitializeComponent();
            _viewModel = new SearchCustomerViewModel();
            BindingContext = _viewModel;
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
                CurrentPage = Locator.SearchCustomer,
                PageViewModel = _viewModel,
                IsModal = true
            };
        }
    }
}