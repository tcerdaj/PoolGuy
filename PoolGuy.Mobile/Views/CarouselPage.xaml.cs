using PoolGuy.Mobile.ViewModels;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarouselPage : ContentPage
    {
        CarouselViewModel viewModel;
        public CarouselPage()
        {
            InitializeComponent();
            viewModel = new CarouselViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void Carousel_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            if (e.CurrentItem is Page page)
            {
                viewModel.Title = page.Title;
                if (page.Title.Equals("SearchCustomer") && !(Application.Current.MainPage).ToolbarItems.Any())
                {
                    (Application.Current.MainPage).ToolbarItems.Add(new ToolbarItem { Text = "Add" });
                }
            }
        }
    }
}