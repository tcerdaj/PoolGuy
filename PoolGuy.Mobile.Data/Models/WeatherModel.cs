﻿using System;

namespace PoolGuy.Mobile.Data.Models
{
    public class WeatherModel : EntityBase
    {
        public int WeatherId { get; set; }
        public int DT { get; set; }
        public int Sunrise { get; set; }

        public int Sunset { get; set; }

        public float Temp { get; set; }
        public float FeelsLike { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public float Uvi { get; set; }
        public int Clouds { get; set; }
        public int Visibility { get; set; }
        public float WindSpeed { get; set; }
        public int WindDeg { get; set; }
        public float Rain { get; set; }
        public string ThreeHoursRain { get; set; }
        private string _icon;
        public string Icon 
        {
            get 
            {
                if (!_icon.Contains("w"))
                    return $"w{_icon}";
                else return _icon;
            }
            set { _icon = value; }
        }
        public string Description { get; set; }
    }
}