using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchCustomerPage : ContentPage
    {
        SearchCustomerViewModel viewModel;
        public SearchCustomerPage()
        {
            InitializeComponent();
            viewModel = new SearchCustomerViewModel();
            BindingContext = viewModel;
            
        }
    }
}