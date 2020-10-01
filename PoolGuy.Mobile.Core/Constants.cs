using SQLite;
using System;
using System.IO;

namespace PoolGuy.Mobile.Data
{
    public class Constants
    {
        public const string DatabaseFilename = "LocalSQLite.db3";

        public const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseFilename);
    }
}