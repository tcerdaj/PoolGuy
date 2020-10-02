using PoolGuy.Mobile.ViewModels;

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
