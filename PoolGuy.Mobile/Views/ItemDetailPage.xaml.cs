using System.ComponentModel;
using Xamarin.Forms;
using PoolGuy.Mobile.ViewModels;

namespace PoolGuy.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}