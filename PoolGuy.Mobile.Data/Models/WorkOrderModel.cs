using System;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.Data.Models
{
    public class WorkOrderModel :  EntityBase
    {
        [ForeignKey(typeof(CustomerModel))]
        public Guid CustomerId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public CustomerModel Customer { get; set; }
        [ForeignKey(typeof(UserModel))]
        public Guid UserId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public UserModel User { get; set; }
        private string _additionalInformation;
        public string AdditionalInformation
        {
            get { return _additionalInformation; }
            set { _additionalInformation = value; OnPropertyChanged("AdditionalInformation"); }
        }

        private ObservableCollection<WorkOrderItemModel> _items;
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ObservableCollection<WorkOrderItemModel> Items
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
    }
}