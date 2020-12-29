using PoolGuy.Mobile.Data.Models.Weather;
using Refit;
using System.Threading.Tasks;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.Services.Interface
{
    public interface IWeatherService
    {
        [Get("/data/2.5/weather?lat={latitude}&lon={longitude}&units={units}&appid={apikey}")]
        Task<WeatherRoot> GetWeather(double latitude, double longitude, string apikey, Units units = Units.Imperial);
        [Get("/data/2.5/weather?q={city}&units={units}&appid={apikey}")]
        Task<WeatherRoot> GetWeather(string city, string apikey, Units units = Units.Imperial);
        [Get("/data/2.5/forecast?q={city}&units={units}&appid={apikey}")]
        Task<WeatherHistoryRoot> GetForecast(string city, string apikey, Units units = Units.Imperial);
        [Get("/data/2.5/forecast?lat={latitude}&lon={longitude}&units={units}&appid={apikey}")]
        Task<WeatherHistoryRoot> GetForecast(double latitude, double longitude, string apikey, Units units = Units.Imperial);
        [Get("/data/2.5/onecall?lat={latitude}&lon={longitude}&exclude=minutely,hourly&units={units}&appid={apikey}")]
        Task<WeatherHistoryRoot> GetOneCall(double latitude, double longitude, string apikey, Units units = Units.Imperial);
    }
}
