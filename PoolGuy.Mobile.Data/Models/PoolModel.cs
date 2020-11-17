using static PoolGuy.Mobile.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace PoolGuy.Mobile.Data.Models
{
    public class PoolModel : EntityBase
    {
        public Guid CustumerId { get; set; }

        private PoolType? _type;
        [Required]
        [EnumDataType(typeof(PoolType))]
        public PoolType? Type { 
            get { return _type; } 
            set { _type = value; OnPropertyChanged("Type"); } 
        }
        
        private double? _surface;
        [Required]
        public double? Surface {
            get { return _surface; }
            set { _surface = value; OnPropertyChanged("Surface"); } 
        }
        
        private double? _capacity;
        [Required]
        public double? Capacity {
            get { return _capacity; }
            set { _capacity = value; OnPropertyChanged("Capacity"); }
        }

        private ObservableCollection<EquipmentModel> _equipments;
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ObservableCollection<EquipmentModel> Equipments
        {
            get => _equipments;
            set { _equipments = value; OnPropertyChanged("Equipments"); }
        }

        public void RaiseEquipmentNotification()
        {
            OnPropertyChanged("Equipments");
        }

        public void RaiseAllNotification()
        {
            OnPropertyChanged("Type");
            OnPropertyChanged("Name");
            OnPropertyChanged("Description");
            OnPropertyChanged("Surface");
            OnPropertyChanged("Capacity");
            OnPropertyChanged("Equipments");
        }
    }
}