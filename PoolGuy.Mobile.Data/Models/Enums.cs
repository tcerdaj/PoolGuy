using System;
using System.Collections.Generic;
using System.Text;

namespace PoolGuy.Mobile.Data.Models
{
    public class Enums
    {
        public enum ModifyType
        {
            None,
            Editing,
            Adding
        }

        public enum WorkStatus
        {
            None,
            Pending,
            Working,
            Completed
        }

        public enum PoolType
        {
            SweetPool,
            SaltPool
        }

        public enum eResultStatus
        {
            None,
            Ok,
            Error
        }

        public enum ePage
        {
            Login,
            Logout,
            Home,
            SearchCustomer,
            Customer,
            SelectEquipment,
            SelectManufacture,
            Equipment,
            EquipmentSelectModel,
            Scheduler,
            WorkOrderDetails,
            WorkOrderList,
            Settings,
            AddEquipment,
            Map
        }

        public enum Units
        {
            Imperial,
            Metric
        }

        public enum ImageType
        {
            Customer,
            User,
            WorkOrder,
            Equipment,
            Report
        }
    }
}