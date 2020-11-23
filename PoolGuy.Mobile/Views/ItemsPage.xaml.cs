using Xamarin.Forms;

using PoolGuy.Mobile.ViewModels;

namespace PoolGuy.Mobile.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        protected override void OnAppearingAsync()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}