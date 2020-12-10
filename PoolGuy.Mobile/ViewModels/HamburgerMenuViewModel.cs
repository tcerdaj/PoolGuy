using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using PoolGuy.Mobile.Extensions;

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
            MenuItems = new ObservableCollection<MenuItemModel>(new[] {
                    new MenuItemModel { Id = 0, Title = Enums.ePage.Home.ToString(), Icon = $"{Enums.ePage.Home.ToString().ToLower()}.png", NavigateToCommand = NavigateToCommand },
                    new MenuItemModel { Id = 1, Title = Enums.ePage.SearchCustomer.ToString().SplitWord(), Icon = $"{Enums.ePage.SearchCustomer.ToString().ToLower()}.png", NavigateToCommand = NavigateToCommand },
                    new MenuItemModel { Id = 2, Title = Enums.ePage.Scheduler.ToString(), Icon = $"{Enums.ePage.Scheduler.ToString().ToLower()}.png", NavigateToCommand = NavigateToCommand},
                    new MenuItemModel { Id = 3, Title = Enums.ePage.Logout.ToString(), Icon = $"{Enums.ePage.Logout.ToString().ToLower()}.png", NavigateToCommand = NavigateToCommand },
                    new MenuItemModel { Id = 4, Title = Enums.ePage.Settings.ToString(), Icon = $"{Enums.ePage.Settings.ToString().ToLower()}.png", NavigateToCommand = NavigateToCommand },
                });
        }

        public ICommand NavigateToCommand
        {
            get
            {
                return new RelayCommand<MenuItemModel>(async(item) =>
                {
                    var masterDetailPage = ((MasterDetailPage)App.Current.MainPage);
                    masterDetailPage.IsPresented = false;
                    await NavigationService.ReplaceRoot($"{item.Title.Replace(" ","")}Page");
                });
            }
        }
    }
}
