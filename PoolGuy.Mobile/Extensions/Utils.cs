using Plugin.Permissions.Abstractions;
using PoolGuy.Mobile.Services.Interface;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Essentials;
using GalaSoft.MvvmLight.Ioc;
using System;
using Xamarin.Forms.GoogleMaps;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using PoolGuy.Mobile.Data.Models;
using Xamarin.Forms;

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
        /// Split the word Customer by Search Customer
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

        /// <summary>
        /// Reorder the custumer list according the distance
        /// </summary>
        /// <param name="selectedCustomers"></param>
        /// <returns></returns>
        public static async Task<List<CustomerModel>> GetReorderedCustomers(this List<CustomerModel> selectedCustomers)
        {
            if (selectedCustomers == null 
                || ( selectedCustomers != null && !selectedCustomers.Any()))
            {
                return new List<CustomerModel>();
            }
            
            var orderedList = new List<Tuple<object, Location>>();

            // Geocode address
            foreach (var cust in selectedCustomers)
            {
                if (!string.IsNullOrEmpty(cust.Address?.FullAddress))
                {
                    var poss = await cust.Address.FullAddress.GetPositionAsync();
                    if (poss != null)
                    {
                        cust.Latitude = poss.Value.Latitude;
                        cust.Longitude = poss.Value.Longitude;
                        orderedList.Add(new Tuple<object, Location>(cust, new Location(cust.Latitude, cust.Longitude)));
                    }
                }
            }

            // Reorder by distance and set distance between customers
            var orderedResult = OrderByDistance(Globals.BranchLocation, orderedList);
            int _ind = 0;
            foreach (var ordered in orderedResult)
            {
                ((CustomerModel)ordered.Item1).Distance = ordered.Item3;
                ((CustomerModel)ordered.Item1).Index = _ind++;
            }

            selectedCustomers = orderedResult
                .Select(x => x.Item1)
                .Cast<CustomerModel>()
                .ToList<CustomerModel>();
            return selectedCustomers;
        }

        /// <summary>
        /// Order my points by distance from mayor to menor
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="pointList"></param>
        /// <returns></returns>
        private static List<Tuple<object, Location, double>> OrderByDistance(Location startPoint, List<Tuple<object, Location>> pointList)
        {
            var orderedList = new List<Tuple<object, Location, double>>();
            LinkedList<Location> points = new LinkedList<Location>(pointList.Select(x => x.Item2).ToList());
            var closetPointItem = GetNearestPoint(startPoint, points);
            var currentPoint = pointList.FirstOrDefault(x => x.Item2 == closetPointItem.Item1);

            while (pointList.Count > 1)
            {
                orderedList.Add(new Tuple<object, Location, double>(currentPoint.Item1, currentPoint.Item2, closetPointItem.Item2));
                pointList.RemoveAt(pointList.FindIndex(x => x.Item1 == currentPoint.Item1));
                closetPointItem = GetNearestPoint(currentPoint.Item2, new LinkedList<Location>(pointList.Select(x => x.Item2).ToList()));
                currentPoint = pointList.FirstOrDefault(x => x.Item2 == closetPointItem.Item1);
            }
            // Add the last point.
            orderedList.Add(new Tuple<object, Location, double>(currentPoint.Item1, currentPoint.Item2, closetPointItem.Item2));
            return orderedList;
        }

        /// <summary>
        /// Get the nearest point from point list
        /// </summary>
        /// <param name="toPoint"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        private static Tuple<Location, double> GetNearestPoint(Location toPoint, LinkedList<Location> points)
        {
            Location nearestPoint = null;
            double minDist2 = double.MaxValue;
            foreach (Location p in points)
            {
                double dist2 = Location.CalculateDistance(p.Latitude,
                    p.Longitude, toPoint.Latitude, toPoint.Longitude, DistanceUnits.Miles);

                if (dist2 > minDist2)
                    continue;

                dist2 = Location.CalculateDistance(p.Latitude,
                    p.Longitude, toPoint.Latitude, toPoint.Longitude, DistanceUnits.Miles);

                if (dist2 < minDist2)
                {
                    minDist2 = dist2;
                    nearestPoint = p;
                }
            }
            return new Tuple<Location, double>(nearestPoint, minDist2);
        }
        
        /// <summary>
        /// Decode the google map direction polyline 
        /// </summary>
        /// <param name="encodedPoints"></param>
        /// <returns></returns>
        public static List<Position> DecodePolyline(this string encodedPoints)
        {
            if (string.IsNullOrWhiteSpace(encodedPoints))
            {
                return null;
            }

            int index = 0;
            var polylineChars = encodedPoints.ToCharArray();
            var poly = new List<Position>();
            int currentLat = 0;
            int currentLng = 0;
            int next5Bits;

            while (index < polylineChars.Length)
            {
                // calculate next latitude
                int sum = 0;
                int shifter = 0;

                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                {
                    break;
                }

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                // calculate next longitude
                sum = 0;
                shifter = 0;

                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5Bits >= 32)
                {
                    break;
                }

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                var mLatLng = new Position(Convert.ToDouble(currentLat) / 100000.0, Convert.ToDouble(currentLng) / 100000.0);
                poly.Add(mLatLng);
            }

            return poly;
        }

        /// <summary>
        /// Return the center from manies points
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static MapSpan FromPositions(this IEnumerable<Position> positions)
        {
            double minLat = double.MaxValue;
            double minLon = double.MaxValue;
            double maxLat = double.MinValue;
            double maxLon = double.MinValue;

            foreach (var p in positions)
            {
                minLat = Math.Min(minLat, p.Latitude);
                minLon = Math.Min(minLon, p.Longitude);
                maxLat = Math.Max(maxLat, p.Latitude);
                maxLon = Math.Max(maxLon, p.Longitude);
            }

            return new MapSpan(
                new Position((minLat + maxLat) / 2d, (minLon + maxLon) / 2d),
                (maxLat - minLat) * 1.15,
                (maxLon - minLon) * 1.15);
        }

        /// <summary>
        /// Determine font color based on background color, if you use background color black return white for textcolor
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ContrastColor(this Color color)
        {
            int d = 0;

            // Counting the perceptive luminance - human eye favors green color... 
            double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            if (luminance > 0.5)
                d = 0; // bright colors - black font
            else
                d = 255; // dark colors - white font

            return Color.FromRgb(d, d, d);
        }
    }
}