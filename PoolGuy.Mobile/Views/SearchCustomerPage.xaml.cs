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

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void SwipeItem_Invoked(object sender, System.EventArgs e)
        {
            
            if (sender is SwipeItem swipeItem)
            {
                
                
                if (swipeItem.Parent is SwipeItems swipeItems)
                {
                    if (swipeItems.Parent is SwipeView swipeView)
                    {
                        if (swipeView.Parent is Frame frame)
                        {
                            if (frame.Parent is StackLayout stackLayout)
                            {
                                var cell = SearchResults.GetChildElements(new Point(stackLayout.Bounds.Center.X, stackLayout.Bounds.Center.Y));
                            }
                        }
                        
                    }
                }
            }
        }
    }
}