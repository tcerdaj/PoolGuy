using static PoolGuy.Mobile.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System;
using SQLite;
using DataAnnotation = System.ComponentModel.DataAnnotations;

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

        public void RaiseAllNotification()
        {
            NotifyPropertyChanged("Type");
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("Description");
            NotifyPropertyChanged("Surface");
            NotifyPropertyChanged("Capacity");
        }
    }
}