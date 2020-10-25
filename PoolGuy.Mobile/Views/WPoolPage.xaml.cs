using CommonServiceLocator;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WPoolPage : ContentPage
    {
        CustomerViewModel _viewModel;
        public WPoolPage()
        {
            InitializeComponent();
            _viewModel = ServiceLocator.Current.GetInstance<CustomerViewModel>();
            BindingContext = _viewModel;
        }
    }
}