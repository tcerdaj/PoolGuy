﻿using System;
using DataAnnotation = System.ComponentModel.DataAnnotations;
using static PoolGuy.Mobile.Data.Models.Enums;
using SQLite;
using System.ComponentModel.DataAnnotations;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Data.Models
{
    public class CustomerModel : EntityBase
    {
        public CustomerModel()
        {

        }

        [ManyToMany(typeof(CustomerSchedulerModel), CascadeOperations = CascadeOperation.All)]
        public List<SchedulerModel> Scheduler { get; set; }

        private List<StopModel> _stops;
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<StopModel> Stops 
        {
            get { return _stops; }
            set { _stops = value; OnPropertyChanged(nameof(Stops)); } 
        }

        [ForeignKey(typeof(AddressModel))]
        public Guid HomeAddressId { get; set; }

        /// <summary>
        /// Home Address
        /// </summary>
        private AddressModel _homeAddress;
        [OneToOne("HomeAddressId", CascadeOperations = CascadeOperation.All)]
        public AddressModel HomeAddress
        {
            get => _homeAddress;
            set { _homeAddress = value; OnPropertyChanged("HomeAddress"); }
        }

        [ForeignKey(typeof(AddressModel))]
        public Guid AddressId { get; set; }

        /// <summary>
        /// Billing Address
        /// </summary>
        private AddressModel _address;
        [OneToOne("AddressId", CascadeOperations = CascadeOperation.All)]
        public AddressModel Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged("Address"); }
        }

        [ForeignKey(typeof(ContactModel))]
        public Guid ContactId { get; set; }

        private ContactModel _contactInfomartion;
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public ContactModel Contact
        {
            get => _contactInfomartion;
            set { _contactInfomartion = value; OnPropertyChanged("ContactInformation"); }
        }

        [ForeignKey(typeof(PoolModel))]
        public Guid PoolId { get; set; }

        private PoolModel _pool;
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public PoolModel Pool
        {
            get { return _pool; }
            set { _pool = value; OnPropertyChanged("Pool"); }
        }

        [JsonIgnore]
        public string Name {
            get { return $"{FirstName} {LastName}"; }
        }
        
        private string _firstName;
        [Unique(Name = "UniqueName", Order = 0, Unique = true)]
        [Required, DataAnnotation.MaxLength(20)]
        public string FirstName 
        {
            get { return _firstName; }
            set { 
                _firstName = value; 
                OnPropertyChanged("FirstName");
            }
        }
        
        private string _lastName;
        [Required, DataAnnotation.MaxLength(20)]
        [Display(Name = "Last Name")]
        [Unique(Name = "UniqueName", Order = 1, Unique = true)]
        public string LastName 
        {
            get { return _lastName; }
            set { 
                _lastName = value; 
                OnPropertyChanged("LastName");
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; OnPropertyChanged("ImageUrl"); }
        }

        private bool _active = true;
        public bool Active 
        {
            get { return _active; }
            set { _active = value; OnPropertyChanged("Active"); }
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
                    DateLastVisit = DateTime.Now.ToUniversalTime();
                }
            }
        }

        private DateTime _dateLastPaid;
        public DateTime DateLastPaid 
        {
            get { return _dateLastPaid; }
            set { _dateLastPaid = value; OnPropertyChanged("DateLastPaid"); }
        }

        private DateTime? _dateLastVisit;
        public DateTime? DateLastVisit 
        {
            get { return _dateLastVisit; }
            set { _dateLastVisit = value; OnPropertyChanged("DateLastVisit"); }
        }

        private double _balance;
        public double Balance 
        {
            get { return _balance; }
            set { _balance = value; OnPropertyChanged("Balance"); }
        }

        private double _latitude;
        public double Latitude 
        {
            get { return _latitude; }
            set { _latitude = value; OnPropertyChanged("Latitude"); }
        }

        private double _logitude;
        public double Longitude 
        {
            get { return _logitude; }
            set { _logitude = value; OnPropertyChanged("Longitude"); } 
        }

        private string _additionalInformation;
        [DataAnnotation.MaxLength(200)]
        [Display(Name = "Additional Information")]
        public string AdditionalInformation 
        {
            get { return _additionalInformation; }
            set { 
                _additionalInformation = value; 
                OnPropertyChanged("AdditionalInformation");
            } 
        }

        private int _customerIndex;
        public int CustomerIndex
        {
            get { return _customerIndex; }
            set { _customerIndex = value; OnPropertyChanged("CustomerIndex"); }
        }

        public void IncreaseIndex(int lastIndex)
        {
            CustomerIndex = lastIndex + 1;
        }

        private double _distance;
        public double Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                OnPropertyChanged("Distance");
            }
        }

        private bool _sameHomeAddress = true;
        public bool SameHomeAddress
        {
            get { return _sameHomeAddress; }
            set { _sameHomeAddress = value; OnPropertyChanged("SameHomeAddress"); }
        }

        [Ignore]
        public Color StatusColor
        {
            get 
            {
                Color color;
                switch (Status)
                {
                    case WorkStatus.None:
                        color = Color.White;
                        break;
                    case WorkStatus.Pending:
                        color = Color.LightGray;
                        break;
                    case WorkStatus.Working:
                        color = Color.Orange;
                        break;
                    case WorkStatus.Completed:
                        color = Color.LightGreen;
                        break;
                    default:
                        color = Color.Red;
                        break;
                }

                return color;
            }
        }
    }
}