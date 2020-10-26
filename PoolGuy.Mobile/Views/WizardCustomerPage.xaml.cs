﻿using CommonServiceLocator;
using PoolGuy.Mobile.Resources;
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
            _viewModel.InitPages();
            BindingContext = _viewModel;
            _primaryColor  =  (Color)Application.Current.Resources["Primary"];
            _unselectedColor = (Color)Application.Current.Resources["UnselectedColor"];
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
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
            else // Save Customer bundle
            {
                _viewModel.SaveCommand.Execute(null);
            }
        }

        private void Carousel_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            PreviusButton.Opacity = _viewModel.Position > 0? 1:.5;
            NextButton.ImageSource = new FontImageSource
            {
                Glyph = (_viewModel.Position + 1) < _viewModel.Pages.Count ?
                MaterialDesignIcons.ChevronRight :
                MaterialDesignIcons.Plus,
                FontFamily = Device.RuntimePlatform == Device.iOS? 
                "Material Design Icons": 
                "materialdesignicons.ttf#Material Design Icons",
                Size = 40,
                Color = _primaryColor
            };
            //NextButton.Opacity = (_viewModel.Position + 1) < _viewModel.Pages.Count? 1:.5;

            if (e.CurrentItem is CustomerPageViewModel page)
            {
                _viewModel.Title = page.Title;
               
                if (string.IsNullOrEmpty(_viewModel.Title))
                {
                    return;
                }

                // Reset color
                headerCustomer.TextColor = _unselectedColor;
                headerAddress.TextColor  = _unselectedColor;
                headerContact.TextColor  = _unselectedColor;
                headerPool.TextColor     = _unselectedColor;

                // Assing to active page
                switch (_viewModel.Title)
                {
                    case "Customer":
                        headerCustomer.TextColor = _primaryColor;
                        break;
                    case "Address":
                        headerAddress.TextColor = _primaryColor;
                        break;
                    case "Contact":
                        headerContact.TextColor = _primaryColor;
                        break;
                    case "Pool":
                        headerPool.TextColor = _primaryColor;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}