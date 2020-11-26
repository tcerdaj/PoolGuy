using DataAnnotation = System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace PoolGuy.Mobile.Data.Models
{
    public class SchedulerModel : EntityBase
    {
        [Required, DataAnnotation.MaxLength(40)]
        public string LongName { get; set; }
        [Required, DataAnnotation.MaxLength(20)]
        public string ShortName { get; set; }
        public int MaxEntries { get; set; }
        public int Index { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        [ForeignKey(typeof(UserModel))]
        public Guid UserId { get; set; }
        [OneToOne]
        public UserModel User { get; set; }

        [ManyToMany(typeof(CustomerModel))]
        public List<CustomerModel> Customers { get; set; }

        public void IncreaseIndex(int lastIndex)
        {
            Index = lastIndex + 1;
        }
    }

    public class CustomerScheduler
    { 
        [ForeignKey(typeof(SchedulerModel))]
        public Guid SchedulerId { get; set; }
        [ForeignKey(typeof(CustomerModel))]
        public Guid CustomerId { get; set; }
    }
}