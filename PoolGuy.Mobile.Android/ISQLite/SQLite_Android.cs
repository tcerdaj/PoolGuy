using PoolGuy.Mobile.Data;
using PoolGuy.Mobile.Droid.ISQLite;
using SQLite;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_Android))]
namespace PoolGuy.Mobile.Droid.ISQLite
{
    public class SQLite_Android : Data.SQLite.ISQLite
    {
        public SQLite_Android()
        {
        }

        SQLiteAsyncConnection Data.SQLite.ISQLite.GetConnection()
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentsPath, Data.Constants.DatabaseFilename);
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(path, Constants.Flags);
            return connection;
        }
    }
}