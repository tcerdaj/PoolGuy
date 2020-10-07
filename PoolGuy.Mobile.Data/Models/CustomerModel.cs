using System;
using DataAnnotation = System.ComponentModel.DataAnnotations;
using static PoolGuy.Mobile.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using PoolGuy.Mobile.Data.Attributes;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using SQLite;

namespace PoolGuy.Mobile.Data.Models
{
    public class CustomerModel : EntityBase
    {
        public CustomerModel()
        {
          
        }

        public Guid PoolID { get; set; }

        public string Name { 
            get { return $"{FirstName} {LastName}"; } 
        }
        private string _firstName;
        [DataAnnotation.Required, DataAnnotation.MaxLength(20)]
        public string FirstName 
        {
            get { return _firstName; }
            set { _firstName = value; NotifyPropertyChanged("FirstName"); }
        }
        
        private string _lastName;
        [DataAnnotation.Required, DataAnnotation.MaxLength(20)]
        public string LastName 
        {
            get { return _lastName; }
            set { _lastName = value; NotifyPropertyChanged("LastName"); }
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
            set { _address2 = value; NotifyPropertyChanged("Address2"); }
        }

        private string _city;
        [DataAnnotation.Required, DataAnnotation.MaxLength(50)]
        public string City 
        {
            get { return _city; }
            set { _city = value; NotifyPropertyChanged("City"); }
        }

        private string _zip;
        [DataAnnotation.Required, DataAnnotation.MaxLength(15)]
        public string Zip 
        {
            get { return _zip; }
            set { _zip = value; NotifyPropertyChanged("Zip"); }
        }

        private string _state;
        [DataAnnotation.Required, DataAnnotation.MaxLength(50)]
        public string State 
        {
            get { return _state; }
            set { _state = value; NotifyPropertyChanged("State"); }
        }

        private byte[] _photo;
        public byte[] Photo 
        {
            get { return _photo; }
            set { _photo = value; NotifyPropertyChanged("Photo"); }
        }

        private string _phone;
        [DataAnnotation.Required, DataAnnotation.MaxLength(10)]
        public string Phone 
        {
            get { return _phone; }
            set { _phone = value; NotifyPropertyChanged("Phone"); }
        }

        private string _email;
        [DataAnnotation.Required, DataAnnotation.MaxLength(200)]
        public string Email 
        {
            get { return _email; }
            set { _email = value; NotifyPropertyChanged("Email"); }
        }

        private bool _active;
        public bool Active 
        {
            get { return _active; }
            set { _active = value; NotifyPropertyChanged("Active"); }
        }

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

        private DateTime _dateLastPaid;
        public DateTime DateLastPaid 
        {
            get { return _dateLastPaid; }
            set { _dateLastPaid = value; NotifyPropertyChanged("DateLastPaid"); }
        }

        private DateTime _dateLastVisit;
        public DateTime DateLastVisit 
        {
            get { return _dateLastVisit; }
            set { _dateLastVisit = value; NotifyPropertyChanged("DateLastVisit"); }
        }

        private double _balance;
        public double Balance 
        {
            get { return _balance; }
            set { _balance = value; NotifyPropertyChanged("Balance"); }
        }

        private double _latitude;
        public double Latitude 
        {
            get { return _latitude; }
            set { _latitude = value; NotifyPropertyChanged("Latitude"); }
        }

        private double _logitude;
        public double Longitude 
        {
            get { return _logitude; }
            set { _logitude = value; NotifyPropertyChanged("Longitude"); } 
        }

        private string _additionalInformation;
        [DataAnnotation.Required, DataAnnotation.MaxLength(200)]
        public string AdditionalInformation 
        {
            get { return _additionalInformation; }
            set { _additionalInformation = value; NotifyPropertyChanged("AdditionalInformation"); } 
        }
        
        private PoolModel _pool;
        
        [Ignore]
        [ValidateObject]
        public PoolModel Pool 
        {
            get { return _pool; }
            set { _pool = value; NotifyPropertyChanged("Pool"); } 
        }
    }
}