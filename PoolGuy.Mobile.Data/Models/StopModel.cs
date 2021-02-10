using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.Data.Models
{
    public class StopModel : EntityBase
    {
        [ForeignKey(typeof(CustomerModel))]
        public Guid CustimerId { get; set; }

        private CustomerModel _customer;
        [OneToOne("CustimerId", CascadeOperations = CascadeOperation.CascadeRead)]
        public CustomerModel Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private UserModel _user;
        public Guid UserId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public UserModel User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged("User"); }
        }

        private string _additionalInformation;
        public string AdditionalInformation
        {
            get { return _additionalInformation; }
            set { _additionalInformation = value; OnPropertyChanged("AdditionalInformation"); }
        }

        private ObservableCollection<StopItemModel> _items;
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ObservableCollection<StopItemModel> Items
        {
            get => _items;
            set { _items = value; OnPropertyChanged("Items"); }
        }

        [Ignore]
        public List<EntityImageModel> Images { get; set; }

        WorkStatus _status;
        public WorkStatus Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        [Ignore]
        public int WeekOfYear
        {
            get 
            {
                if (Created.HasValue)
                {
                    DateTime createdDate = Created.Value;
                    DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Created.Value);
                    if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                    {
                        createdDate = createdDate.AddDays(3);
                    }

                    return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(createdDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                }

                return 0;
            }
        }
    }
}