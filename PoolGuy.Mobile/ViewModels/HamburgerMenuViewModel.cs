using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using PoolGuy.Mobile.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using PoolGuy.Mobile.Extensions;
using System;
using System.Linq;

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
                    new MenuItemModel { Id = 0, 
                        Title = Enums.ePage.Home.ToString(), 
                        Icon = $"{Enums.ePage.Home.ToString().ToLower()}.png", 
                        NavigateToCommand = NavigateToCommand
                    },
                    new MenuItemModel { Id = 1,
                        Title = Enums.ePage.Stops.ToString(),
                        Icon = "stop.png",
                        NavigateToCommand = NavigateToCommand
                    },
                    new MenuItemModel { Id = 2, 
                        Title = Enums.ePage.SearchCustomer.ToString().SplitWord(), 
                        Icon = $"{Enums.ePage.SearchCustomer.ToString().ToLower()}.png", 
                        NavigateToCommand = NavigateToCommand 
                    },
                    new MenuItemModel { Id = 3, 
                        Title = Enums.ePage.Scheduler.ToString(), 
                        Icon = $"{Enums.ePage.Scheduler.ToString().ToLower()}.png", 
                        NavigateToCommand = NavigateToCommand
                    },
                    new MenuItemModel { Id = 4, 
                        Title = Enums.ePage.Logout.ToString(), 
                        Icon = $"{Enums.ePage.Logout.ToString().ToLower()}.png", 
                        NavigateToCommand = NavigateToCommand 
                    },
                    new MenuItemModel { Id = 5, 
                        Title = Enums.ePage.Settings.ToString(), 
                        Icon = $"{Enums.ePage.Settings.ToString().ToLower()}.png", 
                        NavigateToCommand = NavigateToCommand 
                    },
                });

            SubscribeMessage();
        }

        private void SubscribeMessage()
        {
            Notify.SubscribeHamburgerMenuAction((sender) =>
            {
                foreach (var item in MenuItems)
                {
                    item.BackgroundColor = Color.White;
                    item.TextColor = (Color)Application.Current.Resources["Title"];
                }

                var pageItem = MenuItems.FirstOrDefault(x => x.Title.Contains(sender.Arg.SplitWord()));
                if (pageItem != null)
                {
                    pageItem.BackgroundColor = (Color)Application.Current.Resources["Primary"];
                    pageItem.TextColor = Color.Black;
                    pageItem.RaiseColorFiled();
                }

                MenuItems = new ObservableCollection<MenuItemModel>(MenuItems);
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
                    string page = (item.Title.Equals(Enums.ePage.Logout.ToString()) 
                    ? Enums.ePage.Login.ToString() 
                    : item.Title).Replace(" ", "");

                    if (page.Equals(Enums.ePage.Login.ToString()))
                    {
                        Globals.CurrentPage = Enums.ePage.Login;
                        await NavigationService.NavigateToDialog($"{page}Page");
                    }
                    else if(Enum.TryParse(page, true, out Enums.ePage _page))
                    {
                        Globals.CurrentPage = _page;
                        await NavigationService.ReplaceRoot($"{page}Page");
                    }
                });
            }
        }
    }
}
