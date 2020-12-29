using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.GoogleMap;
using PoolGuy.Mobile.Services.Interface;
using System.Threading.Tasks;
using System;
using Refit;
using Xamarin.Forms;
using PoolGuy.Mobile.Services;

[assembly: Dependency(typeof(GoogleMapService))]
namespace PoolGuy.Mobile.Services
{
    public class GoogleMapService : IGoogleMapService
    {
        const string _baseUrl = "https://maps.googleapis.com";
        /// <summary>
        /// Get Google map direcctions based in stops
        /// </summary>
        /// <param name="startPoint">Start point can be the brach location or techitial fisical address or enter by the user</param>
        /// <param name="endPoint">End point can be the brach location or techitial fisical address or enter by the user</param>
        /// <param name="apiKey"></param>
        /// <param name="units">Use imperial or metric, imperial by default</param>
        /// <returns>Direction</returns>
        public async Task<Direction> GetDirections(string startPoint, string endPoint, string apiKey, Enums.Units units = Enums.Units.Imperial)
        {
            try
            {
                var request = RestService.For<IGoogleMapService>(_baseUrl);
                var response = await request
                    .GetDirections(startPoint, endPoint, apiKey)
                    .ConfigureAwait(false);
                
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Google map direcctions based in stops
        /// </summary>
        /// <param name="startPoint">Start point can be the brach location or techitial fisical address or enter by the user</param>
        /// <param name="endPoint">End point can be the brach location or techitial fisical address or enter by the user</param>
        /// <param name="stops">Represent the tech stops in the route is reparated by | </param>
        /// <param name="apiKey"></param>
        /// <param name="units">Use imperial or metric, imperial by default</param>
        /// <returns>Direction</returns>
        public async Task<Direction> GetDirections(string startPoint, string endPoint, string stops, string apiKey, Enums.Units units = Enums.Units.Imperial)
        {
            try
            {
                var request = RestService.For<IGoogleMapService>(_baseUrl);
                var response = await request
                    .GetDirections(startPoint, endPoint, stops, apiKey)
                    .ConfigureAwait(false);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Google map direcctions based in stops with route optimization
        /// Caution: Requests using waypoint optimization are billed at a higher rate. Learn more about how Google Maps Platform products are billed.
        /// </summary>
        /// <param name="startPoint">Start point can be the brach location or techitial fisical address or enter by the user</param>
        /// <param name="endPoint">End point can be the brach location or techitial fisical address or enter by the user</param>
        /// <param name="stops">Represent the tech stops in the route is reparated by | </param>
        /// <param name="avoid">tolls|highways|ferries|</param>
        /// <param name="apiKey"></param>
        /// <param name="units">Use imperial or metric, imperial by default</param>
        /// <returns>Direction</returns>
        public async Task<Direction> GetOptimizeDirections(string startPoint, string endPoint, string stops, string avoid, string apiKey, Enums.Units units = Enums.Units.Imperial)
        {
            try
            {
                var request = RestService.For<IGoogleMapService>(_baseUrl);
                var response = await request
                    .GetOptimizeDirections(startPoint, endPoint, stops, avoid, apiKey)
                    .ConfigureAwait(false);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}