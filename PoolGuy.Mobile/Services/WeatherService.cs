using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.Weather;
using PoolGuy.Mobile.Services;
using PoolGuy.Mobile.Services.Interface;
using Refit;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(WeatherService))]
namespace PoolGuy.Mobile.Services
{
    public class WeatherService : IWeatherService
    {
        const string _baseUrl = "http://api.openweathermap.org";
        /// <summary>
        /// Access current weather data for any location on Earth including over 200,000 cities! We collect and process weather data from different sources such as global and local weather models, satellites, radars and vast network of weather stations.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        public async Task<WeatherRoot> GetWeather(double latitude, double longitude, Enums.Units units = Enums.Units.Imperial)
        {
            try
            {
                var request = RestService.For<IWeatherService>(_baseUrl);
                var response = await request.GetWeather(latitude, longitude).ConfigureAwait(false);
                return response;
            }
            catch (System.Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Access current weather data for any location on Earth including over 200,000 cities! We collect and process weather data from different sources such as global and local weather models, satellites, radars and vast network of weather stations.
        /// </summary>
        /// <param name="city"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        public async Task<WeatherRoot> GetWeather(string city, Enums.Units units = Enums.Units.Imperial)
        {
            if (string.IsNullOrEmpty(city))
            {
                return null;
            }

            try
            {
                var request = RestService.For<IWeatherService>(_baseUrl);
                var response = await request.GetWeather(city).ConfigureAwait(false);
                return response;
            }
            catch (System.Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Hourly forecast by OpenWeatherMap! Hourly forecast for 4 days, with 96 timestamps and higher geographic accuracy.
        /// </summary>
        /// <param name="city"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        public async Task<WeatherHistoryRoot> GetForecast(string city, Enums.Units units = Enums.Units.Imperial)
        {
            try
            {
                var request = RestService.For<IWeatherService>(_baseUrl);
                var response = await request.GetForecast(city).ConfigureAwait(false);
                return response;
            }
            catch (System.Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Hourly forecast by OpenWeatherMap! Hourly forecast for 4 days, with 96 timestamps and higher geographic accuracy.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        public async Task<WeatherHistoryRoot> GetForecast(double latitude, double longitude, Enums.Units units = Enums.Units.Imperial)
        {
            try
            {
                var request = RestService.For<IWeatherService>(_baseUrl);
                var response = await request.GetForecast(latitude, longitude).ConfigureAwait(false);
                return response;
            }
            catch (System.Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// The One Call API provides the following weather data for any geographical coordinates:
        /// Current weather
        /// Minute forecast for 1 hour
        /// Hourly forecast for 48 hours
        /// Daily forecast for 7 days
        /// Government weather alerts
        /// Historical weather data for the previous 5 days
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        public async Task<WeatherHistoryRoot> GetOneCall(double latitude, double longitude, Enums.Units units = Enums.Units.Imperial)
        {
            try
            {
                var request = RestService.For<IWeatherService>(_baseUrl);
                var response = await request.GetOneCall(latitude, longitude).ConfigureAwait(false);
                return response;
            }
            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content,
                // which is type of ProblemDetails according to RFC 7807

                // If the response contains additional properties on the problem details,
                // they will be added to the validationException.Content.Extensions collection.
                throw;
            }
            catch (ApiException exception)
            {
                // other exception handling
                throw;
            }
            catch (System.Exception e)
            {
                throw;
            }
        }
    }
}