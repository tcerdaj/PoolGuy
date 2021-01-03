﻿using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PoolGuy.Mobile.Data.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static bool ContainsKey(string key)
        {
            return AppSettings.Contains(key);
        }

        #region Constans
        private const string NavigationMetadataKey = "navigationMetaData_key";
        private static readonly string NavigationMetadataDefault = string.Empty;
        private const string TabletsRegisteredKey = "tabletsRegistered_key";
        private static readonly string TabletsRegisteredDefault = null;
        private const string IsLoggedInKey = "isLoggedIn_Key";
        private static readonly bool IsLoggedInDefault = false;
        private const string IsLoggingKey = "isLogging_Key";
        #endregion
        #region Properties
        public static string NavigationMetadata
        {
            get => AppSettings.GetValueOrDefault(NavigationMetadataKey, NavigationMetadataDefault);
            set => AppSettings.AddOrUpdateValue(NavigationMetadataKey, value);
        }

        public static string[] TabletsRegistered
        {
            get => AppSettings.GetValueOrDefault(TabletsRegisteredKey, TabletsRegisteredDefault) == null? new string[] { }
            : AppSettings.GetValueOrDefault(TabletsRegisteredKey, TabletsRegisteredDefault).Split(',');
            set => AppSettings.AddOrUpdateValue(TabletsRegisteredKey, value == null? null : string.Join(",", value));
        }

        public static bool IsLoggedIn
        {
            get => AppSettings.GetValueOrDefault(IsLoggedInKey, IsLoggedInDefault);
            set => AppSettings.AddOrUpdateValue(IsLoggedInKey, value);
        }

        public static bool IsLoggingIn
        {
            get => AppSettings.GetValueOrDefault(IsLoggingKey, IsLoggedInDefault);
            set => AppSettings.AddOrUpdateValue(IsLoggingKey, value);
        }
        #endregion
    }
}
