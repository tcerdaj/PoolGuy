using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Views
{
    public partial class CustomerPage : ContentPage
    {
        CustomerViewModel _viewModel;
        public CustomerPage()
        {
            InitializeComponent();
            _viewModel = new CustomerViewModel(this);
            BindingContext = _viewModel;
        }
    }
}