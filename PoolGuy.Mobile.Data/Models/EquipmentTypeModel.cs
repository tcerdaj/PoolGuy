using System;
using System.Collections.Generic;
using System.Text;

namespace PoolGuy.Mobile.Data.Models
{
    public class EquipmentTypeModel : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
