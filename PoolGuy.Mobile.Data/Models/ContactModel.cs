using DataAnnotation = System.ComponentModel.DataAnnotations;
using System;
using SQLite;

namespace PoolGuy.Mobile.Data.Models
{
    public class ContactModel : EntityBase
    {
        public Guid CustomerId { get; set; }

        private string _phone;
        [DataAnnotation.Required, DataAnnotation.MaxLength(10), Unique]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; NotifyPropertyChanged("Phone");  }
        }

        private string _cellPhone;
        [DataAnnotation.MaxLength(10)]
        public string CellPhone
        {
            get { return _cellPhone; }
            set { _cellPhone = value; NotifyPropertyChanged("CellPhone");  }
        }

        private string _email;
        [DataAnnotation.Required, DataAnnotation.MaxLength(200), Unique]
        public string Email
        {
            get { return _email; }
            set { _email = value; NotifyPropertyChanged("Email");  }
        }
    }
}
