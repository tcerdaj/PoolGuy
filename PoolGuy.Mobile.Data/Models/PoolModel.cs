using static PoolGuy.Mobile.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PoolGuy.Mobile.Data.Models
{
    public class PoolModel: EntityBase
    {
        private PoolType? _type;
        [Required]
        [EnumDataType(typeof(PoolType))]
        public PoolType? Type { 
            get { return _type; } 
            set { _type = value; NotifyPropertyChanged("Type"); } 
        }
        private string _name;
        [MaxLength(40)]
        public string Name { 
            get { return _name; } 
            set { _name = value; NotifyPropertyChanged("Name"); } 
        }
        private string _description;
        [MaxLength(200)]
        public string Description { 
            get { return _description; } 
            set { _description = value; NotifyPropertyChanged("Description"); } 
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