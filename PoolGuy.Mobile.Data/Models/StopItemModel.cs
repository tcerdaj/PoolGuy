using static PoolGuy.Mobile.Data.Models.Enums;
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
        public eItemType ItemType { get; set; }
        public string Value { get; set; }
        public string Test { get; set; }
        public string Appliyed { get; set; }
        public string Suggested { get; set; }
        public decimal Price { get; set; }
        public int Frequency { get; set; }
        public bool IsRequired { get; set; }
        public eFrequencyType FrequencyType { get; set; }
        [ForeignKey(typeof(StopModel))]
        public Guid StopId { get; set; }
        private StopModel _stop;
        [ManyToOne]
        public StopModel Stop
        {
            get { return _stop; }
            set { _stop = value; OnPropertyChanged("Stop"); }
        }
    }
}