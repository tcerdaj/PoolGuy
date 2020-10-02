using SQLite;

namespace PoolGuy.Mobile.Data.SQLite
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}