using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services.Interface;
using Rg.Plugins.Popup.Pages;
using PoolGuy.Mobile.ViewModels;

namespace PoolGuy.Mobile.Views
{
    public partial class ActionSheetPopupPage : PopupPage, IContentPage
    {
        ActionSheetPopupViewModel _viewModel;

        public ActionSheetPopupPage()
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
            _viewModel = new ActionSheetPopupViewModel();
            BindingContext = _viewModel;
        }

        public ActionSheetPopupPage(ActionSheetModel model)
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
            _viewModel = new ActionSheetPopupViewModel(model);
            BindingContext = _viewModel;
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