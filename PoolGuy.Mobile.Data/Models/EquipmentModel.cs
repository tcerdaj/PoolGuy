using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace PoolGuy.Mobile.Data.Models
{
    public class EquipmentModel : EntityBase
    {
        private string _imageUrl;
        public string ImageUrl 
        {
            get 
            { 
                if (string.IsNullOrEmpty(_imageUrl) && Type != null) {
                    return Type.ImageUrl;
                }

                return _imageUrl;
            }
            set { _imageUrl = value; OnPropertyChanged("ImageUrl"); } 
        }
        public Guid CustomerId { get; set; }

        [ForeignKey(typeof(PoolModel))]
        public Guid PoolId { get; set; }
        private PoolModel _pool;
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public PoolModel Pool
        {
            get { return _pool; }
            set { _pool = value; OnPropertyChanged("Pool"); }
        }

        [ForeignKey(typeof(EquipmentTypeModel))]
        [Unique(Name = "UniqueModel", Order = 0, Unique = true)]
        public Guid EquipmentTypeId { get; set; }
        private EquipmentTypeModel _type;
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public EquipmentTypeModel Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("Type"); }
        }

        [ForeignKey(typeof(ManufactureModel))]
        [Unique(Name = "UniqueModel", Order = 1, Unique = true)]
        public Guid ManufactureId { get; set; }
        private ManufactureModel _manufacture;
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public ManufactureModel Manufacture
        {
            get { return _manufacture; }
            set { _manufacture = value; OnPropertyChanged("Manufacture"); }
        }

        private string _model;
        
        [Unique(Name ="UniqueModel", Order = 2, Unique =true)]
        public string Model { get { return _model; } set { _model = value; OnPropertyChanged("Model"); } }

        private string _serialNumber;
        [Unique(Name = "UniqueSerialNumber", Order = 0, Unique = true)]
        public string SerialNumber { get { return _serialNumber; } set { _serialNumber = value; OnPropertyChanged("SerialNumber"); } }
        private DateTime? _dateInstalled;
        public DateTime? DateInstalled { get { return _dateInstalled; } set { _dateInstalled = value; OnPropertyChanged("DateInstalled"); } }
        private DateTime? _warrantyExpiration;
        public DateTime? WarrantyExpiration { get { return _warrantyExpiration; } set { _warrantyExpiration = value; OnPropertyChanged("WarrantyExpiration"); } }
        private DateTime? _lastMaintenance;
        public DateTime? LastMaintenance { get { return _lastMaintenance; } set { _lastMaintenance = value; OnPropertyChanged("LastMaintenance"); } }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
    }
}
