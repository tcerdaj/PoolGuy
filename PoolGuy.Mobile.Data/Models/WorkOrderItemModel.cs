using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoolGuy.Mobile.Data.Models
{
    public class WorkOrderItemModel : EntityBase
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal AlternatePrice { get; set; }
        public decimal Total 
        {
            get 
            { 
               return Math.Round(Price * Quantity, 2, MidpointRounding.AwayFromZero); 
            }
         }

        public decimal AlternateTotal
        {
            get
            {
                return Math.Round(AlternatePrice * Quantity, 2, MidpointRounding.AwayFromZero);
            }
        }

        [ForeignKey(typeof(EquipmentModel))]
        public Guid EquipmentId { get; set; }
        [OneToOne("EquipmentId", CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
        public EquipmentModel Equipment { get; set; }
    }
}
