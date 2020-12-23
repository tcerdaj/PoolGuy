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
using System.IO;
using System.Threading;

namespace PoolGuy.Mobile.Extensions
{
    public static class Utils
    {
        public static byte[] ToByteArray(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

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
                    await DependencyService.Get<IUserDialogs>().DisplayAlertAsync("Geocode Address", "No internet connectivity is available now, check airplain mode if apply your addres is not be setted to geocode.", "Ok");
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

        /// <summary>
        /// Split the word SearchCustomer by Search Customer
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SplitWord(this string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

        /// <summary>
        /// Convert long bytes to string in B, KB, MB,etc..
        /// </summary>
        /// <param name="byteCount"></param>
        /// <returns></returns>
        public static string BytesToString(this long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
            {
                return "0" + suf[0];
            }

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{(Math.Sign(byteCount) * num)} {suf[place]}";
        }

        public static async Task<T> WaitOrCancel<T>(this Task<T> task, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await Task.WhenAny(task, token.WhenCanceled());
            token.ThrowIfCancellationRequested();

            return await task;
        }

        public static Task WhenCanceled(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }
    }
}