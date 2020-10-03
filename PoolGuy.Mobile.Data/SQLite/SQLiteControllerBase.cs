using System;
using SQLite;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Data.SQLite
{
    public class SQLiteControllerBase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            var conn = DependencyService.Get<ISQLite>().GetConnection();
            return conn;
        });

        static readonly Lazy<SQLiteConnection> _lazyInitializer = new Lazy<SQLiteConnection>(() =>
        {
            return new SQLiteConnection(Constants.DatabasePath, Constants.Flags);
        });

        public static SQLiteAsyncConnection DatabaseAsync => lazyInitializer.Value;

        public static SQLiteConnection Database = _lazyInitializer.Value;
    }
}