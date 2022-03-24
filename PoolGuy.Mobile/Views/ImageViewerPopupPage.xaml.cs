using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using Rg.Plugins.Popup.Pages;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageViewerPopupPage : PopupPage, IContentPage
    {
        ImageViewerPopupViewModel _viewModel;

        public ImageViewerPopupPage()
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
            _viewModel = new ImageViewerPopupViewModel();
            BindingContext = _viewModel;
        }

        public ImageViewerPopupPage(string imageUrl)
        {
            try
            {
                InitializeComponent();
                CloseWhenBackgroundIsClicked = false;
                _viewModel = new ImageViewerPopupViewModel(imageUrl);
                BindingContext = _viewModel;

            }
            catch (System.Exception ex)
            {
                throw;
            }        
        }

        protected override bool OnBackgroundClicked()
        {
            //_viewModel.SelectActionCommand.Execute(_viewModel.CancelLabel);
            return true;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Initialize();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CleanUp();
        }

        public void Initialize()
        {
        }

        public MobileNavigationModel OnSleep()
        {
            return null;
        }

        public void CleanUp()
        {
        }
    }
}