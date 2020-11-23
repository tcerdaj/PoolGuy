using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;
using System;

namespace PoolGuy.Mobile.Models
{

    public class WeatherHistoryRoot
    {
        public int WeatherId { get; set; }
        public float lat { get; set; }
        public int lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        [ForeignKey(typeof(Current))]
        public int CurrentId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Current current { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public Daily[] daily { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public Alert[] alerts { get; set; }
    }

    public class Current
    {
        public DateTime dt { get; set; }
        public DateTime sunrise { get; set; }
        public DateTime sunset { get; set; }
        public float temp { get; set; }
        public float feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public float dew_point { get; set; }
        public float uvi { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public float wind_speed { get; set; }
        public int wind_deg { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public WeatherModel[] weather { get; set; }
    }

    public class Daily
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        [ForeignKey(typeof(Temp))]
        public Guid TempId { get; set; }
        
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Temp temp { get; set; }
        
        [ForeignKey(typeof(Feels_Like))]
        public Guid FeelsLikeId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Feels_Like feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public float dew_point { get; set; }
        public float wind_speed { get; set; }
        public int wind_deg { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public WeatherModel[] weather { get; set; }
        public int clouds { get; set; }
        public float pop { get; set; }
        public float uvi { get; set; }
        public float rain { get; set; }
        [ForeignKey(typeof(Temp))]
        public Guid RainId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Rain Rain { get; set; }
    }

    public class Temp
    {
        public float day { get; set; }
        public float min { get; set; }
        public float max { get; set; }
        public float night { get; set; }
        public float eve { get; set; }
        public float morn { get; set; }
    }

    public class Feels_Like
    {
        public float day { get; set; }
        public float night { get; set; }
        public float eve { get; set; }
        public float morn { get; set; }
    }

    public class Alert
    {
        public string sender_name { get; set; }
        public string _event { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public string description { get; set; }
    }

    public class Rain
    {
        [JsonProperty("3h")]
        public string ThreeHours { get;set; }
    }
}