using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Runtime.Serialization;

namespace PoolGuy.Mobile.Data.Models
{
    [DataContract]
    public class EquipmentModel : EntityBase
    {
        private string _imageUrl;
       [DataMember]
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
        [OneToOne(CascadeOperations = CascadeOperation.None)]
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
        [DataMember]
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
        [DataMember]
        public ManufactureModel Manufacture
        {
            get { return _manufacture; }
            set { _manufacture = value; OnPropertyChanged("Manufacture"); }
        }

        private string _model;
        
        [Unique(Name ="UniqueModel", Order = 2, Unique =true)]
        [DataMember]
        public string Model { get { return _model; } set { _model = value; OnPropertyChanged("Model"); } }

        private string _serialNumber;
        [Unique(Name = "UniqueSerialNumber", Order = 0, Unique = true)]
        [DataMember]
        public string SerialNumber { get { return _serialNumber; } set { _serialNumber = value; OnPropertyChanged("SerialNumber"); } }
        private DateTime? _dateInstalled;
        [DataMember]
        public DateTime? DateInstalled { get { return _dateInstalled; } set { _dateInstalled = value; OnPropertyChanged("DateInstalled"); } }
        private DateTime? _warrantyExpiration;
        [DataMember]
        public DateTime? WarrantyExpiration { get { return _warrantyExpiration; } set { _warrantyExpiration = value; OnPropertyChanged("WarrantyExpiration"); } }
        private DateTime? _lastMaintenance;
        [DataMember]
        public DateTime? LastMaintenance { get { return _lastMaintenance; } set { _lastMaintenance = value; OnPropertyChanged("LastMaintenance"); } }
        [DataMember]
        public string Field1 { get; set; }
        [DataMember]
        public string Field2 { get; set; }
    }
}
