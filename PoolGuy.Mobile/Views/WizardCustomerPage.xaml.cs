using CommonServiceLocator;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizardCustomerPage : ContentPage
    {
        CustomerViewModel _viewModel;
        Color _primaryColor, _unselectedColor;
        public WizardCustomerPage()
        {
            InitializeComponent();
            _viewModel = ServiceLocator.Current.GetInstance<CustomerViewModel>();
            BindingContext = _viewModel;
            _primaryColor  =  (Color)Application.Current.Resources["Primary"];
            _unselectedColor = (Color)Application.Current.Resources["UnselectedColor"];
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.InitPages();
        }

        private void PreviusButton_Clicked(object sender, System.EventArgs e)
        {
            if (_viewModel.Position > 0)
            {
                if (_viewModel.IsValid())
                {
                    _viewModel.Position = Carousel.Position - 1;
                }
            }
        }

        private void NextButton_Clicked(object sender, System.EventArgs e)
        {
            if ((_viewModel.Position + 1) < _viewModel.Pages.Count)
            {
                if (_viewModel.IsValid())
                {
                    _viewModel.Position = Carousel.Position + 1;
                }
            }
        }

        private void Carousel_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            PreviusButton.Opacity = _viewModel.Position > 0? 1:.5;
            NextButton.Opacity = (_viewModel.Position + 1) < _viewModel.Pages.Count? 1:.5;

            if (e.CurrentItem is Page page)
            {
                _viewModel.Title = page.Title;

                if (string.IsNullOrEmpty(_viewModel.Title))
                {
                    return;
                }

                // Reset color
                headerCustomer.TextColor = _unselectedColor;
                headerCustomer.FontAttributes = FontAttributes.None;
             
                headerAddress.TextColor  = _unselectedColor;
                headerAddress.FontAttributes = FontAttributes.None;

                headerContact.TextColor  = _unselectedColor;
                headerContact.FontAttributes = FontAttributes.None;

                headerPool.TextColor     = _unselectedColor;
                headerPool.FontAttributes = FontAttributes.None;

                // Assing to active page
                switch (_viewModel.Title)
                {
                    case "Customer":
                        headerCustomer.TextColor = _primaryColor;
                        headerCustomer.FontAttributes = FontAttributes.Bold;
                        break;
                    case "Address":
                        headerAddress.TextColor = _primaryColor;
                        headerAddress.FontAttributes = FontAttributes.Bold;
                        break;
                    case "Contact":
                        headerContact.TextColor = _primaryColor;
                        headerContact.FontAttributes = FontAttributes.Bold;
                        break;
                    case "Pool":
                        headerPool.TextColor = _primaryColor;
                        headerPool.FontAttributes = FontAttributes.Bold;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}