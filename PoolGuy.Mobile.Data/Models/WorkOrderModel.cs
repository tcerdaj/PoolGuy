using System;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace PoolGuy.Mobile.Data.Models
{
    public class WorkOrderModel :  EntityBase
    {
        [ForeignKey(typeof(CustomerModel))]
        public Guid CustomerId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public CustomerModel Customer { get; set; }
        [ForeignKey(typeof(UserModel))]
        public Guid UserId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public UserModel User { get; set; }
        public int TotalItems { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<WorkOrderDetailsModel> WorkOrderDetails { get; set; }
    }
    
    public class WorkOrderDetailsModel : EntityBase
    {
        public int Index { get; set; }

        [ForeignKey(typeof(WorkOrderModel))]
        public Guid WorkOrderId { get; set; }
        [ManyToOne]
        public WorkOrderModel WorkOrder { get; set; }

        [ForeignKey(typeof(ItemModel))]
        public Guid ItemId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public ItemModel Item { get; set; }
        public string TestValue { get; set; }
        public string OptimalValue { get; set; }
        public string FinalValue { get; set; }
        public void IncreaseIndex(int lastIndex)
        {
            Index = lastIndex + 1;
        }
    }

    public class ItemModel : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCheckField { get; set; }
    }
}
