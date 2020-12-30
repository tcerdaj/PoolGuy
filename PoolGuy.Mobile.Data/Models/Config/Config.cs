using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PoolGuy.Mobile.Data.Models.Config
{
    public class Config
    {
        public static ApiKey[] ApiKeys
        {
            get
            {
                try
                {
                    var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Config)).Assembly;
                    var stream = assembly.GetManifestResourceStream("PoolGuy.Mobile.Data.Models.Config.apikeys.json");
                    using (var reader = new StreamReader(stream))
                    {
                        var json = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(json))
                        {
                            return JsonConvert.DeserializeObject<ApiKey[]>(json);
                        }
                    }

                }
                catch (Exception ex) 
                {
                    Debug.WriteLine(ex);
                }

                return null;
            }
        }
    }

    public class ApiKey
    { 
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
    }
}