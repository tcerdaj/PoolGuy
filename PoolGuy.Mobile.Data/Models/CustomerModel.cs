using System;
using DataAnnotation = System.ComponentModel.DataAnnotations;
using static PoolGuy.Mobile.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using PoolGuy.Mobile.Data.Attributes;

namespace PoolGuy.Mobile.Data.Models
{
    public class CustomerModel : EntityBase
    {
        public CustomerModel()
        {
           if(Id.Equals(Guid.Empty))
           {
                Pool = new PoolModel();
           }
        }

        public string Name { get; set; }
        [DataAnnotation.Required, DataAnnotation.MaxLength(20)]
        public string FirstName { get; set; }
        [DataAnnotation.Required, DataAnnotation.MaxLength(20)]
        public string LastName { get; set; }
        [DataAnnotation.Required, DataAnnotation.MaxLength(80)]
        public string Address1 { get; set; }
        [DataAnnotation.MaxLength(80)]
        public string Address2 { get; set; }
        [DataAnnotation.Required, DataAnnotation.MaxLength(50)]
        public string City { get; set; }
        [DataAnnotation.Required, DataAnnotation.MaxLength(15)]
        public string Zip { get; set; }
        [DataAnnotation.Required, DataAnnotation.MaxLength(50)]
        public string State { get; set; }
        public byte[] Photo { get; set; }
        [DataAnnotation.Required, DataAnnotation.MaxLength(10)]
        public string Phone { get; set; }
        [DataAnnotation.Required, DataAnnotation.MaxLength(200)]
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
        [DataAnnotation.Required, DataAnnotation.MaxLength(200)]
        public string AdditionalInformation { get; set; }
        [ValidateObject]
        public PoolModel Pool { get; set; }
    }
}