using PoolGuy.Mobile.Views;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PoolGuy.Mobile.ViewModels
{
    public class WizardCustomerModel : BaseViewModel
    {
        public WizardCustomerModel()
        {
            Pages = new List<Page> { 
              new HomePage(),
              new CustomerPage(),
              new SearchCustomerPage()
            };
        }
        private List<Page> _pages;

        public List<Page> Pages
        {
            get { return _pages; }
            set { _pages = value; OnPropertyChanged("Pages"); }
        }

    }
}