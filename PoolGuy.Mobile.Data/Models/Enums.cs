
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
            None,
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
            None,
            Imperial,
            Metric
        }

        public enum ImageType
        {
            None,
            Customer,
            User,
            WorkOrder,
            Equipment,
            Report,
            Pool
        }

        public enum ePageType
        {
            None,
            PushPopup,
            Dialog,
            ReplaceRoot,
            Popup,
            CloseModal,
            PopPopup
        }

        public enum eVolumeType
        {
            None,
            Litre = 0,
            Pint = 1,
            Gallon = 2,
            VPM,
            Unit,
            Temperature,
            Device
        }

        public enum eMassUnit
        {
            None,
            Grams,
            Kilograms,
            Ounces,
            Pounds
        }

        public enum eItemType
        {
            Normal,
            Stop,
            PartRequest
        }

        public enum eFrequencyType
        {
            Daily,
            Weekly,
            Monthly,
            Annually
        }
    }
}