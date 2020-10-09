using PoolGuy.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HamburgerMenuPage : ContentPage
    {
        HamburgerMenuViewModel viewModel;
        public CollectionView ListView;
        public HamburgerMenuPage()
        {
            InitializeComponent();
            viewModel = new HamburgerMenuViewModel();
            BindingContext = viewModel;
            ListView = MenuItemsListView;
        }
    }
}