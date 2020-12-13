using PoolGuy.Mobile.Helpers;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile
{
    public static class Globals
    {
        private static ePage _currentPage;
        public static ePage CurrentPage 
        { 
            get => _currentPage;
            set 
            {
                _currentPage = value;
                Notify.RaiseHamburgerMenuAction(
                    new Messages.RefreshMessage { Arg = _currentPage.ToString() });
            }
        }
    }
}
