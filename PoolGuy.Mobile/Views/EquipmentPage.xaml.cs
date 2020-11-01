using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EquipmentPage : ContentPage
    {
        EquipmentViewModel _viewModel;
        public EquipmentPage(EquipmentModel equipment)
        {
            InitializeComponent();
            _viewModel = new EquipmentViewModel(equipment);
            _viewModel.SetView(this);
            BindingContext = _viewModel;
        }
    }
}