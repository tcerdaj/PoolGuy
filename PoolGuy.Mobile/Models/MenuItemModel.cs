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

        private Color _backGroundcolor;
        public Color BackgroundColor 
        {
            get { return _backGroundcolor; }
            set { _backGroundcolor = value; OnPropertyChanged("BackgroundColor"); }
        }

        private Color _textColor;
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; OnPropertyChanged("TextColor"); }
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
