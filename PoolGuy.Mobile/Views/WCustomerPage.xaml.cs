using CommonServiceLocator;
using PoolGuy.Mobile.CustomControls;
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
    public partial class WCustomerPage : ContentPage
    {
        CustomerViewModel _viewModel;
        public WCustomerPage()
        {
            InitializeComponent();
            _viewModel = ServiceLocator.Current.GetInstance<CustomerViewModel>();
            _viewModel.SetView(this);
        }

        public WCustomerPage(CustomerViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _viewModel.SetView(this);
        }

        private  void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel == null)
            { 
               _viewModel = ServiceLocator.Current.GetInstance<CustomerViewModel>();
            }

            _viewModel.TakePhotoCommand.Execute(null);
        }
    }
}