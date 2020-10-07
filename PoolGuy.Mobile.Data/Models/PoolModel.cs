using static PoolGuy.Mobile.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PoolGuy.Mobile.Data.Models
{
    public class PoolModel: EntityBase
    {
        [Required]
        [EnumDataType(typeof(PoolType))]
        [DefaultValue(PoolType.Saltwater)]
        public PoolType? Type { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public double? Surface { get; set; }
        [Required]
        public double? Capacity { get; set; }

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