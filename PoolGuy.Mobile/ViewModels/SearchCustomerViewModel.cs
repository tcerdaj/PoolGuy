using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoolGuy.Mobile.ViewModels
{
    public class SearchCustomerViewModel : BaseViewModel
    {
        public SearchCustomerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "").Replace("Search", "");
        }

        private CustomerModel _customer = new CustomerModel() {Pool = new PoolModel()};

        public CustomerModel Customer
        {
            get { return _customer; } 
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(() => AddAsync()); }
        }

        private void AddAsync()
        {
            
        }
    }
}