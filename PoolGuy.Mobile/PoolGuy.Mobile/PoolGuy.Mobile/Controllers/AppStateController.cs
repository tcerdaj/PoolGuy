using PoolGuy.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Xamarin.Forms;
using PoolGuy.Mobile.Services.Interface;
using System.Diagnostics;
using PoolGuy.Mobile.Helpers;
using System.IO;
using PoolGuy.Mobile.ViewModels;

namespace PoolGuy.Mobile.Controllers
{
    public static class AppStateController
    {
        private const string CurrentWorkOrderKey = "CurrentWorkOrder";
        private const string DidSleepKey = "DidSleep";
        private static Stack<MobileNavigationModel> _navigationMetaStack = new Stack<MobileNavigationModel>();
        private static XmlSerializer _serializer = null;

        public static int DidSleep => DependencyService.Get<ISimpleCache>().Get<int>(DidSleepKey);

        public static bool SaveViewState(MobileNavigationModel mobileNavigationModel)
        {
            var success = false;

            try
            {
                if (mobileNavigationModel == null)
                {
                    return false;
                }

                _navigationMetaStack.Push(mobileNavigationModel);

                success = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return success;
        }

        public static void PopViewState()
        {
            try
            {
                if (!_navigationMetaStack.Any())
                {
                    return;
                }

                _navigationMetaStack.Pop();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static bool SaveFinalState()
        {
            var success = false;
            var cache = DependencyService.Get<ISimpleCache>();

            try
            {
                Settings.NavigationMetadata = SerializeNavigationMetaStack(_navigationMetaStack.ToList());
                cache.Set<int>(DidSleepKey, 6);
                success = true;
            }
            catch (Exception ex)
            {
                Settings.NavigationMetadata = string.Empty;
                Debug.WriteLine(ex);
            }

            return success;
        }

        public static List<MobileNavigationModel> RestoreState()
        {
            var navigationMetaList = new List<MobileNavigationModel>();

            try
            {
                var cache = DependencyService.Get<ISimpleCache>();
                _navigationMetaStack = DeserializeNavigationMetaStack(Settings.NavigationMetadata);
                navigationMetaList = _navigationMetaStack.ToList();
                navigationMetaList.Reverse();
                ResetState();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return navigationMetaList;
        }

        public static void ResetState()
        {
            try
            {
                var cache = DependencyService.Get<ISimpleCache>();
                cache.Set<int>("DidSleep", 0);
                cache.Set<DateTime>("ClockedInDateTime", DateTime.MinValue);
                Settings.NavigationMetadata = string.Empty;

                if (_navigationMetaStack.Any())
                {
                    _navigationMetaStack.Pop();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private static string SerializeNavigationMetaStack(List<MobileNavigationModel> navigationMetaStack)
        {
            try
            {
                if (_serializer == null)
                {
                    _serializer = new XmlSerializer(typeof(List<MobileNavigationModel>), Types.ToArray());
                }

                using (var writer = new StringWriter())
                {
                    _serializer.Serialize(writer, navigationMetaStack);
                    return writer.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        private static Stack<MobileNavigationModel> DeserializeNavigationMetaStack(string serializedNavigationMetaStack)
        {
            try
            {
                if (string.IsNullOrEmpty(serializedNavigationMetaStack))
                {
                    return new Stack<MobileNavigationModel>();
                }

                if (_serializer == null)
                {
                    _serializer = new XmlSerializer(typeof(List<MobileNavigationModel>), Types.ToArray());
                }

                using (var reader = new StringReader(serializedNavigationMetaStack))
                {
                    var list = (List<MobileNavigationModel>)_serializer.Deserialize(reader);
                    list.Reverse();
                    return new Stack<MobileNavigationModel>(list);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public static void ClearNavigationMetaStack()
        {
            try
            {
                _navigationMetaStack.Clear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static List<Type> Types = new List<Type>
        {
            typeof(BaseViewModel),
            typeof(HomeViewModel),
            typeof(CustomerViewModel)
        };
    }
}
