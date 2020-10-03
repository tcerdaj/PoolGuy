namespace PoolGuy.Mobile.Services.Interface
{
    public interface ISimpleCache
    {
        bool Remove(string key);
        bool Add<T>(string key, T value);
        bool Set<T>(string key, T value);
        bool Replace<T>(string key, T value);
        T Get<T>(string key);
    }
}