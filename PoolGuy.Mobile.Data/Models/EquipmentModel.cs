using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Data.Models
{
    public class EquipmentModel : EntityBase
    {
        public Guid PoolId { get; set; }

        public Guid EquipmentTypeId { get; set; }
        public Guid ManufactureId { get; set; }

        public string Model { get; set; }

        public string SerialNumber { get; set; }
        public DateTime DateInstalled { get; set; }
        public DateTime WarrantyExpiration { get; set; }
        public DateTime LastMaintenance { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
    }
}
