using System;
using DataAnnotation = System.ComponentModel.DataAnnotations;
using static PoolGuy.Mobile.Data.Models.Enums;
using SQLite;
using System.ComponentModel.DataAnnotations;
using SQLiteNetExtensions.Attributes;

namespace PoolGuy.Mobile.Data.Models
{
    public class CustomerModel : EntityBase
    {
        public CustomerModel()
        {

        }

        [ForeignKey(typeof(AddressModel))]
        public Guid AddressId { get; set; }

        private AddressModel _address;
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public AddressModel Address
        {
            get => _address;
            set { _address = value; NotifyPropertyChanged("Address"); }
        }

        [ForeignKey(typeof(ContactModel))]
        public Guid ContactId { get; set; }

        private ContactModel _contactInfomartion;
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public ContactModel Contact
        {
            get => _contactInfomartion;
            set { _contactInfomartion = value; NotifyPropertyChanged("ContactInformation"); }
        }

        [ForeignKey(typeof(PoolModel))]
        public Guid PoolId { get; set; }

        private PoolModel _pool;
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public PoolModel Pool
        {
            get { return _pool; }
            set { _pool = value; NotifyPropertyChanged("Pool"); }
        }

        public string Name {
            get { return $"{FirstName} {LastName}"; }
        }
        private string _firstName;
        [Display(Name ="First Name")]
        [DataAnnotation.Required, DataAnnotation.MaxLength(20), Indexed(Name = "CustomerName", Order =2, Unique = true)]
        public string FirstName 
        {
            get { return _firstName; }
            set { 
                _firstName = value; 
                NotifyPropertyChanged("FirstName");
                WasModified = true;
            }
        }
        
        private string _lastName;
        [DataAnnotation.Required, DataAnnotation.MaxLength(20), Indexed(Name = "CustomerName", Order = 1, Unique = true)]
        [Display(Name = "Last Name")]
        public string LastName 
        {
            get { return _lastName; }
            set { 
                _lastName = value; 
                NotifyPropertyChanged("LastName");
                WasModified = true;
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; NotifyPropertyChanged("ImageUrl"); }
        }

        private bool _active = true;
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
        [DataAnnotation.MaxLength(200)]
        [Display(Name = "Additional Information")]
        public string AdditionalInformation 
        {
            get { return _additionalInformation; }
            set { 
                _additionalInformation = value; 
                NotifyPropertyChanged("AdditionalInformation");
                WasModified = true;
            } 
        }

    }
}