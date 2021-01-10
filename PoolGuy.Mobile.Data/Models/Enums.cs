
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
            Map,
            Stops,
            StopDetails
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
            Report,
            Pool
        }

        public enum ePageType
        {
            PushPopup,
            Dialog,
            ReplaceRoot,
            Popup,
            CloseModal,
            PopPopup
        }

        public enum eVolumeType
        {
            Litre = 0,
            Pint = 1,
            Gallon = 2
        }

        public enum eMassUnit
        {
            Grams,
            Kilograms,
            Ounces,
            Pounds
        }
    }
}