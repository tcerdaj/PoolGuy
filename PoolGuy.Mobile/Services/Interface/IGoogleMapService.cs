using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.GoogleMap;
using Refit;
using System.Threading.Tasks;

namespace PoolGuy.Mobile.Services.Interface
{
    public interface IGoogleMapService
    {
        /// <summary>
        /// Get Google map direcctions based in stops
        /// </summary>
        /// <param name="startPoint">Start point can be the brach location or techitial fisical address or enter by the user</param>
        /// <param name="endPoint">End point can be the brach location or techitial fisical address or enter by the user</param>
        /// <param name="apiKey"></param>
        /// <param name="units">Use imperial or metric, imperial by default</param>
        /// <returns></returns>
        [Get("/maps/api/directions/json?origin={startPoint}&destination={endPoint}&departure_time=now&units={units}&key={apiKey}")]
        Task<Direction> GetDirections(string startPoint, string endPoint, string apiKey, Enums.Units units = Enums.Units.Imperial);

        /// <summary>
        /// Get Google map direcctions based in stops
        /// </summary>
        /// <param name="startPoint">Start point can be the brach location or techitial fisical address or enter by the user</param>
        /// <param name="endPoint">End point can be the brach location or techitial fisical address or enter by the user</param>
        /// <param name="stops">Represent the tech stops in the route is reparated by | </param>
        /// <param name="apiKey"></param>
        /// <param name="units">Use imperial or metric, imperial by default</param>
        /// <returns></returns>
        [Get("/maps/api/directions/json?origin={startPoint}&destination={endPoint}&waypoints={stops}&departure_time=now&units={units}&key={apiKey}")]
        Task<Direction> GetDirections(string startPoint, string endPoint, string stops, string apiKey,Enums.Units units = Enums.Units.Imperial);

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
        /// <returns></returns>
        [Get("/maps/api/directions/json?origin={startPoint}&destination={endPoint}&waypoints=optimize:true|{stops}&avoid={avoid}&departure_time=now&units={units}&key={apiKey}")]
        Task<Direction> GetOptimizeDirections(string startPoint, string endPoint, string stops, string avoid, string apiKey, Enums.Units units = Enums.Units.Imperial);
    }
}