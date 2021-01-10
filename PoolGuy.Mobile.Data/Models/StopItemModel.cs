﻿using static PoolGuy.Mobile.Data.Models.Enums;
using System;
using SQLiteNetExtensions.Attributes;

namespace PoolGuy.Mobile.Data.Models
{
    public class StopItemModel : EntityBase
    {
        public int Index { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCheckField { get; set; }
        public eVolumeType VolumeType { get; set; }
        public eMassUnit MassUnit { get; set; }
        public string Value { get; set; }
        public decimal Price { get; set; }
        [ForeignKey(typeof(StopModel))]
        public Guid StopId { get; set; }
        private StopModel _stop;
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public StopModel Stop
        {
            get { return _stop; }
            set { _stop = value; OnPropertyChanged("Stop"); }
        }
    }
}