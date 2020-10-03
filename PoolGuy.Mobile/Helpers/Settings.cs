using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PoolGuy.Mobile.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static bool ContainsKey(string key)
        {
            return AppSettings.Contains(key);
        }

        #region Constans
        public const string NavigationMetadataKey = "navigationMetaData_key";
        private static readonly string NavigationMetadataDefault = string.Empty;
        #endregion
        #region Properties
        public static string NavigationMetadata
        {
            get => AppSettings.GetValueOrDefault(NavigationMetadataKey, NavigationMetadataDefault);
            set => AppSettings.AddOrUpdateValue(NavigationMetadataKey, value);
        }
        #endregion
    }
}
