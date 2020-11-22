using Plugin.Permissions.Abstractions;
using PoolGuy.Mobile.Services.Interface;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Essentials;
using GalaSoft.MvvmLight.Ioc;
using System;
using Xamarin.Forms.GoogleMaps;
using System.Collections.Generic;
using System.Diagnostics;
using PoolGuy.Mobile.Resources;

namespace PoolGuy.Mobile.Extensions
{
    public static class Utils
    {
        /// <summary>
        /// Get Device current position
        /// </summary>
        /// <returns></returns>
        public static async Task<Location> GetPositionAsync()
        {
            try
            {
                var permissionStatus = await DependencyService.Get<IPermissionService>()
                    .CheckPermissions(Permission.Location);

                if (permissionStatus.Any(x => x.Value == Plugin.Permissions.Abstractions.PermissionStatus.Granted))
                {
                    var position = await Geolocation.GetLocationAsync(
                        new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(30)));

                    return position;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await SimpleIoc.Default.GetInstance<IUserDialogs>().DisplayAlertAsync(fnsEx.Message, "GeoLocator", "Ok");
            }
            catch (PermissionException pEx)
            {
                await SimpleIoc.Default.GetInstance<IUserDialogs>().DisplayAlertAsync(pEx.Message, "GeoLocator", "Ok");
            }
            catch (Exception ex)
            {
                await SimpleIoc.Default.GetInstance<IUserDialogs>().DisplayAlertAsync(ex.Message, "GeoLocator", "Ok");
            }

            return null;
        }

        
        /// <summary>
        /// Get address geocode position
        /// </summary>
        /// <param name="fullAddress"></param>
        /// <returns></returns>
        public static async Task<Position?> GetPositionAsync(this string fullAddress)
        {
            try
            {
                if (string.IsNullOrEmpty(fullAddress))
                {
                    return null;
                }

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.None)
                {
                    await Shell.Current.DisplayAlert("Geocode Address", "No internet connectivity is available now, check airplain mode if apply your addres is not be setted to geocode.", "Ok");
                    return null;
                }

                Geocoder geoCoder = new Geocoder();
                IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(fullAddress);
                Position position = approximateLocations.FirstOrDefault();

                return position;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}