﻿using Newtonsoft.Json;
using System;
using DataAnnotation = System.ComponentModel.DataAnnotations;

namespace PoolGuy.Mobile.Data.Models
{
    public class AddressModel : EntityBase
    {

        [JsonIgnore]
        public string FullAddress
        {
            get 
            {
                if (string.IsNullOrEmpty(Address1)
                    && string.IsNullOrEmpty(City)
                      && string.IsNullOrEmpty(State)
                        && string.IsNullOrEmpty(Zip))
                {
                    return string.Empty;
                }


                return $"{Address1} {Address2}, {City}, {State}, {Zip}".Trim();
            }
        }

        private string _address1;
        [DataAnnotation.Required, DataAnnotation.MaxLength(80)]
        public string Address1
        {
            get { return _address1; }
            set { _address1 = value; OnPropertyChanged("Address1"); }
        }

        private string _address2;
        [DataAnnotation.MaxLength(80)]
        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; OnPropertyChanged("Address2");  }
        }

        private string _city;
        [DataAnnotation.Required, DataAnnotation.MaxLength(50)]
        public string City
        {
            get { return _city; }
            set { _city = value; OnPropertyChanged("City");  }
        }

        private string _zip;
        [DataAnnotation.Required, DataAnnotation.MaxLength(15)]
        public string Zip
        {
            get { return _zip; }
            set { _zip = value; OnPropertyChanged("Zip");  }
        }

        private string _state;
        [DataAnnotation.Required, DataAnnotation.MaxLength(50)]
        public string State
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged("State");  }
        }

        private bool _sameHomeAddress;
        public bool SameHomeAddress
        {
            get { return _sameHomeAddress; }
            set { _sameHomeAddress = value; OnPropertyChanged("SameHomeAddress"); }
        }

        public void NotififyAll()
        {
            try
            {
                foreach (var prop in this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    OnPropertyChanged(prop.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void ClearAll()
        {
            try
            {
                foreach (var prop in this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                     prop.SetValue(this, null);
                }

                NotififyAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}