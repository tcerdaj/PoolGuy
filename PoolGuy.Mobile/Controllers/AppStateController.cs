using System;
using System.Linq;
using Xamarin.Forms;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.Data.Helpers;
using PoolGuy.Mobile.ViewModels;
using PoolGuy.Mobile.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using PoolGuy.Mobile.Extensions;
using Newtonsoft.Json;

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

                if (!_navigationMetaStack
                    .Any(x => x.CurrentPage == mobileNavigationModel.CurrentPage ))
                {
                    _navigationMetaStack.Push(mobileNavigationModel);
                }

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
                if (_navigationMetaStack != null)
                {
                    Settings.NavigationMetadata = SerializeNavigationMetaStack(_navigationMetaStack.ToList());
                    cache.Set<int>(DidSleepKey, 6);
                    success = true;
                }
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

                //var json = JsonConvert.SerializeObject(navigationMetaStack, new JsonSerializerSettings
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //    NullValueHandling = NullValueHandling.Ignore,
                //    CheckAdditionalContent = false
                //});,,

                if (_serializer == null)
                {
                     var types = typeof(BaseViewModel).GetAllSubTypes();
                    _serializer = new XmlSerializer(typeof(List<MobileNavigationModel>), Types );
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
                    _serializer = new XmlSerializer(typeof(List<MobileNavigationModel>), new Type[] { typeof(BaseViewModel)});
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

        public static Type[] Types = new Type[]
        {
            typeof(BaseViewModel),
            //typeof(HomeViewModel),
            //typeof(LoginViewModel),
            typeof(SearchCustomerViewModel),
            typeof(CustomerViewModel),
            //typeof(EquipmentViewModel),
            //typeof(SettingsViewModel),
            //typeof(StopsViewModel),
            //typeof(StopDetailsViewModel)
        };
    }
}
