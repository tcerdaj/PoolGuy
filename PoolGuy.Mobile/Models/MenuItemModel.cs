using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Models
{
    public class MenuItemModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MenuItemModel()
        {
            TargetType = typeof(MenuItemModel);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public string Icon { get; set; }

        public Color BackgroundColor 
        {
            get 
            {
                return Globals.CurrentPage.ToString().Contains(Title) ? (Color)Application.Current.Resources["Primary"] : Color.White;
            }
        }

        public Color TextColor
        {
            get
            {
                return Globals.CurrentPage.ToString().Contains(Title) ? Color.White : (Color)Application.Current.Resources["Title"];
            }
        }

        public Type TargetType { get; set; }

        public ICommand NavigateToCommand { get; set; }

        public void RaiseColorFiled()
        {
            OnPropertyChanged("BackgroundColor");
            OnPropertyChanged("TextColor");
        }
    }
}
