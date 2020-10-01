using PoolGuy.Mobile.Data.SQLite;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Data.Controllers
{
    public class BaseController<T>
    {
        static ILocalDataStore<T> _localDataStore => DependencyService.Get<ILocalDataStore<T>>();

        public static ILocalDataStore<T> LocalData
        {
            get { return _localDataStore; }
        }
    }
}
