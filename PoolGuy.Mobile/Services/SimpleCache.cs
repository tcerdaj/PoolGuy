using Newtonsoft.Json;
using PoolGuy.Mobile.Services;
using PoolGuy.Mobile.Services.Interface;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(SimpleCache))]
namespace PoolGuy.Mobile.Services
{
    public class SimpleCache : ISimpleCache
    {
        private readonly JsonSerializerSettings _serializerSettings;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "SCS0028:TypeNameHandling is set to other value than 'None' that may lead to deserialization vulnerability", Justification = "Not used to deserialize JSON from external source")]
        public SimpleCache()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Ignore,
                DateParseHandling = DateParseHandling.DateTime
            };
        }

        public bool Remove(string key)
        {
            try
            {
                return Xamarin.Forms.Application.Current.Properties.Remove(key);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return false;
        }

        public bool Add<T>(string key, T value)
        {
            try
            {
                Xamarin.Forms.Application.Current.Properties.Add(key, JsonConvert.SerializeObject(value, typeof(T), Formatting.None, _serializerSettings));
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return false;
        }

        public bool Set<T>(string key, T value)
        {
            try
            {
                Xamarin.Forms.Application.Current.Properties[key] = JsonConvert.SerializeObject(value, typeof(T), Formatting.None, _serializerSettings); ;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return false;
        }

        public bool Replace<T>(string key, T value)
        {
            try
            {
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(key))
                {
                    Xamarin.Forms.Application.Current.Properties[key] = JsonConvert.SerializeObject(value, typeof(T), Formatting.None, _serializerSettings);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return false;
        }

        public T Get<T>(string key)
        {
            try
            {
                if (Xamarin.Forms.Application.Current.Properties.TryGetValue(key, out object val))
                {
                    return JsonConvert.DeserializeObject<T>(val as string, _serializerSettings);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return default(T);
        }
    }
}