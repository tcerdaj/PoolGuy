using PoolGuy.Mobile.ViewModels;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace PoolGuy.Mobile.Models
{
    public class MobileNavigationModel 
    {
        public string CurrentPage { get; set; }
        public string SubPage { get; set; }
        public bool IsModal { get; set; }
        public BaseViewModel PageViewModel { get; set; }
    }
}