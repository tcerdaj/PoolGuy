using PoolGuy.Mobile.Data.Models;
using System;

namespace PoolGuy.Mobile.Models
{
    public class MobileCustomerModel
    {
        public DateTime SelectedDate { get; set; }
        public CustomerModel Customer { get; set; }
    }
}