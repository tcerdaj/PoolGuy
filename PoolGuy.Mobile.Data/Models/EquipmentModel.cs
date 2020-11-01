using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Data.Models
{
    public class EquipmentModel : EntityBase
    {
        public Guid CustomerId { get; set; }

        [ForeignKey(typeof(PoolModel))]
        public Guid PoolId { get; set; }

        [ForeignKey(typeof(EquipmentTypeModel))]
        public Guid EquipmentTypeId { get; set; }
        private EquipmentTypeModel _type;
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public EquipmentTypeModel Type
        {
            get { return _type; }
            set { _type = value; NotifyPropertyChanged("Type"); }
        }

        [ForeignKey(typeof(ManufactureModel))]
        public Guid ManufactureId { get; set; }
        private ManufactureModel _manufacture;
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public ManufactureModel Manufacture
        {
            get { return _manufacture; }
            set { _manufacture = value; NotifyPropertyChanged("Manufacture"); }
        }

        public string Model { get; set; }

        public string SerialNumber { get; set; }
        public DateTime DateInstalled { get; set; }
        public DateTime WarrantyExpiration { get; set; }
        public DateTime LastMaintenance { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
    }
}
