using static PoolGuy.Mobile.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace PoolGuy.Mobile.Data.Models
{
    public class PoolModel: EntityBase
    {
        public Guid CustomerId { get; set; }

        private PoolType? _type;
        [Required]
        [EnumDataType(typeof(PoolType))]
        public PoolType? Type { 
            get { return _type; } 
            set { _type = value; NotifyPropertyChanged("Type"); } 
        }
        
        private double? _surface;
        [Required]
        public double? Surface {
            get { return _surface; }
            set { _surface = value; NotifyPropertyChanged("Surface"); } 
        }
        
        private double? _capacity;
        [Required]
        public double? Capacity {
            get { return _capacity; }
            set { _capacity = value; NotifyPropertyChanged("Capacity"); }
        }

        private List<EquipmentModel> _equipments;
        [OneToMany]
        public List<EquipmentModel> Equipments
        {
            get => _equipments;
            set { _equipments = value; NotifyPropertyChanged("Equipments"); }
        }

        public void RaiseAllNotification()
        {
            NotifyPropertyChanged("Type");
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("Description");
            NotifyPropertyChanged("Surface");
            NotifyPropertyChanged("Capacity");
            NotifyPropertyChanged("Equipments");
        }
    }
}