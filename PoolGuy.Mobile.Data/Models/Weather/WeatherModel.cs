using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System;

namespace PoolGuy.Mobile.Data.Models.Weather
{
    public class WeatherModel : EntityBase
    {
        public string WeatherJson { get; set; }
        public WeatherHistoryRoot Main 
        {
            get 
            {
                try
                {
                    if (string.IsNullOrEmpty(WeatherJson))
                    {
                        return null;
                    }

                    return JsonConvert.DeserializeObject<WeatherHistoryRoot>(WeatherJson);
                }
                catch (System.Exception e)
                {
                    Debug.WriteLine(e);
                    return null;
                }
            } 
        }

        public Weather Weather 
        {
            get
            {
                try
                {
                    if (Main?.current?.weather == null)
                    {
                        return null;
                    }

                    return Main.current.weather.FirstOrDefault() ?? new Weather();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return null;
                }
            } 
        }
        public float AccumulatedRain 
        {
            get 
            {
                try
                {
                    if (Main == null)
                    {
                        return 0;
                    }

                    return Main.daily.Sum(x => x.rain);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return 0;
                }
            } 
        }
        
        public string Icon 
        {
            get 
            {
                try
                {
                    if (Weather == null || (Weather != null && string.IsNullOrEmpty(Weather.Icon)))
                    {
                        return "";
                    }

                    return $"w{Weather.Icon}";
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return "";
                }
            }
        }

        public void RaiseFields()
        {
            OnPropertyChanged("Main");
            OnPropertyChanged("Weather");
            OnPropertyChanged("AccumulatedRain");
            OnPropertyChanged("Icon");
        }
    }
}