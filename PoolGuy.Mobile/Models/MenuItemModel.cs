using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PoolGuy.Mobile.Models
{
    public class MenuItemModel
    {
        public MenuItemModel()
        {
            TargetType = typeof(MenuItemModel);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }

        public ICommand NavigateToCommand { get; set; }
    }
}
