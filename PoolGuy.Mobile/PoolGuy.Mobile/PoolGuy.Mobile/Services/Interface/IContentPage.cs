using PoolGuy.Mobile.Models;

namespace PoolGuy.Mobile.Services.Interface
{
    public interface IContentPage
    {
        void Initialize();
        MobileNavigationModel OnSleep();
        void CleanUp();
    }
}