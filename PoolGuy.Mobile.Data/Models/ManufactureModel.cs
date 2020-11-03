using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace PoolGuy.Mobile.Data.Models
{
    public class ManufactureModel : EntityBase
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string WebSite { get; set; }
        private bool _selected;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set { _selected = value; OnPropertyChanged("Selected"); }
        }

        public void NotifyAll()
        {
            OnPropertyChanged("Name");
            OnPropertyChanged("Description");
            OnPropertyChanged("ImageUrl");
            OnPropertyChanged("Selected");
            OnPropertyChanged("WebSite");
        }
    }
}
