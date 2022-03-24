using CommonServiceLocator;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WAddressPage : ContentPage
    {
        CustomerViewModel _viewModel;
        public WAddressPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            _viewModel = (CustomerViewModel)BindingContext;
        }

        public WAddressPage(CustomerViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }
    }
}