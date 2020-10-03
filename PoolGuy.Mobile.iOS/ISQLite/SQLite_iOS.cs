using ObjCRuntime;
using PoolGuy.Mobile.Droid.ISQLite;
using SQLite;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace PoolGuy.Mobile.Droid.ISQLite
{
    public class SQLite_iOS : Data.SQLite.ISQLite
    {
        public SQLite_iOS()
        {
        }

        SQLiteAsyncConnection Data.SQLite.ISQLite.GetConnection()
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentsPath, Data.Constants.DatabaseFilename);
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(path);
            return connection;
        }
    }
}