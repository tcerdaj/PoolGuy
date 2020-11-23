using System;

namespace PoolGuy.Mobile.Data.Models
{
    public class WeatherModel : EntityBase
    {
        public DateTime DT { get; set; }
        public DateTime Sunrise { get; set; }

        public DateTime Sunset { get; set; }

        public float Temp { get; set; }
        public float FeelsLike { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public float DewPoint { get; set; }
        public float Uvi { get; set; }
        public int Clouds { get; set; }
        public int Visibility { get; set; }
        public float WindSpeed { get; set; }
        public int WindDeg { get; set; }
        public float Rain { get; set; }
        public float ThreeHoursRain { get; set; }
    }
}