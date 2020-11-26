using System;
using SQLiteNetExtensions.Attributes;

namespace PoolGuy.Mobile.Data.Models
{
    public class ReportModel : EntityBase
    {
        [ForeignKey(typeof(CustomerModel))]
        public Guid CustomerId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public CustomerModel Customer { get; set; }

        [ForeignKey(typeof(EquipmentTypeModel))]
        public Guid EquipmentTypeId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public EquipmentTypeModel EquipmentType { get; set; }
        public string Issues { get;set; }
        public string AdditionalInformation { get; set; }
    }

    public class EquipmentIssueModel: EntityBase
    {
        [ForeignKey(typeof(EquipmentTypeModel))]
        public Guid EquipmentTypeId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public EquipmentTypeModel EquipmentType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}