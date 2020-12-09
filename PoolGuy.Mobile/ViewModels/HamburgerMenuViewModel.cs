using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoolGuy.Mobile.ViewModels
{
    public class HamburgerMenuViewModel: BaseViewModel
    {
        private ObservableCollection<MenuItemModel> _menuItems;
        public ObservableCollection<MenuItemModel> MenuItems 
        {
            get { return _menuItems; }
            set { _menuItems = value; OnPropertyChanged("MenuItems"); }
        }

        public HamburgerMenuViewModel()
        {
            MenuItems = new ObservableCollection<MenuItemModel>(new[]
                {
                    new MenuItemModel { Id = 0, Title = "Home", Icon = "dashboard_black.png", NavigateToCommand = NavigateToCommand },
                    new MenuItemModel { Id = 1, Title = "Customer", Icon = "account_circle.png", NavigateToCommand = NavigateToCommand },
                    new MenuItemModel { Id = 2, Title = "Scheduler", Icon = "schedule.png", NavigateToCommand = NavigateToCommand},
                    new MenuItemModel { Id = 3, Title = "Logout", Icon = "logout.png", NavigateToCommand = NavigateToCommand },
                    new MenuItemModel { Id = 4, Title = "Settings", Icon = "settings.png", NavigateToCommand = NavigateToCommand },
                });
        }

        public ICommand NavigateToCommand
        {
            get
            {
                return new RelayCommand<string>(async(page) =>
                {
                    var masterDetailPage = ((MasterDetailPage)App.Current.MainPage);
                    masterDetailPage.IsPresented = false;
                    await NavigationService.ReplaceRoot($"{page}Page");
                });
            }
        }
    }
}
