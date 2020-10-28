using System;
using DataAnnotation = System.ComponentModel.DataAnnotations;

namespace PoolGuy.Mobile.Data.Models
{
    public class AddressModel : EntityBase
    {
        public Guid CustomerId { get; set; }

        public string FullAddress
        {
            get 
            {
                return $"{Address1} {Address2}, {City}, {State}, {Zip}".Trim();
            }
        }

        private string _address1;
        [DataAnnotation.Required, DataAnnotation.MaxLength(80)]
        public string Address1
        {
            get { return _address1; }
            set { _address1 = value; NotifyPropertyChanged("Address1"); }
        }

        private string _address2;
        [DataAnnotation.MaxLength(80)]
        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; NotifyPropertyChanged("Address2");  }
        }

        private string _city;
        [DataAnnotation.Required, DataAnnotation.MaxLength(50)]
        public string City
        {
            get { return _city; }
            set { _city = value; NotifyPropertyChanged("City");  }
        }

        private string _zip;
        [DataAnnotation.Required, DataAnnotation.MaxLength(15)]
        public string Zip
        {
            get { return _zip; }
            set { _zip = value; NotifyPropertyChanged("Zip");  }
        }

        private string _state;
        [DataAnnotation.Required, DataAnnotation.MaxLength(50)]
        public string State
        {
            get { return _state; }
            set { _state = value; NotifyPropertyChanged("State");  }
        }
    }
}