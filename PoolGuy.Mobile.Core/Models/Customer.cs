using SQLite;
using System;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.Data.Models
{
    public class Customer : EntityBase
    {
        public Customer()
        {

        }

        public Customer(ModifyType modifyType)
        {
            if (modifyType == ModifyType.Adding)
            {
                Id = Guid.NewGuid();
                Created = DateTime.Now;
            }
            else if (modifyType == ModifyType.Editing)
            {
                Modified = DateTime.Now;
            }
        }

        [NotNull]
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotNull]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public byte[] Photo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }

        WorkStatus _status;
        public WorkStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                
                if(_status != WorkStatus.None)
                {
                    DateLastVisit = DateTime.Now;
                }
            }
        }
        public DateTime DateLastPaid { get; set; }
        public DateTime DateLastVisit { get; set; }
        public double Balance { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Pool Pool { get; set; }
    }
}