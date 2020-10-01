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
            None,
            Sweetwater,
            Saltwater
        }

        public enum eResultStatus
        {
            None,
            Ok,
            Error
        }
    }
}