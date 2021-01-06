using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace PoolGuy.Mobile.Data.Models
{
    public class StopModel : EntityBase
    {
        [ForeignKey(typeof(CustomerModel))]
        public Guid CustimerId { get; set; }
      
        private CustomerModel _customer;
        [OneToOne("CustimerId", CascadeOperations = CascadeOperation.All)]
        public CustomerModel Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        public DateTime StopDateTime { get; set; }
       
        private string _additionalInformation;
        public string AdditionalInformation
        {
            get { return _additionalInformation; }
            set { _additionalInformation = value; OnPropertyChanged("AdditionalInformation"); }
        }

        public List<EntityImageModel> Images { get; set; }
    }
}