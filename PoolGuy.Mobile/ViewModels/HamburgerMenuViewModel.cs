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
        public ObservableCollection<MenuItemModel> MenuItems { get; set; }
        public HamburgerMenuViewModel()
        {
            MenuItems = new ObservableCollection<MenuItemModel>(new[]
                {
                    new MenuItemModel { Id = 0, Title = "Home", NavigateToCommand = NavigateToCommand },
                    new MenuItemModel { Id = 1, Title = "Customer", NavigateToCommand = NavigateToCommand },
                    new MenuItemModel { Id = 2, Title = "Page 3", NavigateToCommand = NavigateToCommand},
                    new MenuItemModel { Id = 3, Title = "Page 4", NavigateToCommand = NavigateToCommand },
                    new MenuItemModel { Id = 4, Title = "Page 5", NavigateToCommand = NavigateToCommand },
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
