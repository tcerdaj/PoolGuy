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
using static PoolGuy.Mobile.Data.Models.Enums;
using PoolGuy.Mobile.Data.Controllers;

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

        public static async Task AddStopDetaultItemsAsync()
        {
            try
            {
                var stopItemController = new StopItemController().LocalData;
                var stopItems = await stopItemController.List(new Data.Models.Query.SQLControllerListCriteriaModel
                {
                    Filter = new System.Collections.Generic.List<Data.Models.Query.SQLControllerListFilterField> {
                       new Data.Models.Query.SQLControllerListFilterField {
                            FieldName = "ItemType",
                            ValueLBound = ((int)Enums.eItemType.Stop).ToString()
                       }}
                });

                if (!stopItems.Any())
                {
                    var items = new List<StopItemModel> {
                 new StopItemModel {
                   Index = 0,
                   Name = "CHL",
                   Description ="Chlorine is a chemical element with the symbol Cl and atomic number 17. The second-lightest of the halogens, it appears between fluorine and bromine in the periodic table and its properties are mostly intermediate between them",
                   ImageUrl = "https://www.istockphoto.com/photo/swimming-pool-water-treatment-with-chlorine-tablets-gm182174385-10189979",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Gallon,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 69.99m,
                 },
                 new StopItemModel {
                   Index = 1,
                   Name = "pH",
                   Description ="pH is a measure of hydrogen ion concentration, a measure of the acidity or alkalinity of a solution. The pH scale usually ranges from 0 to 14. Aqueous solutions at 25°C with a pH less than 7 are acidic, while those with a pH greater than 7 are basic or alkaline. A pH level of 7.0 at 25°C is defined as neutral because the concentration of H3O+ equals the concentration of OH− in pure water. Very strong acids might have a negative pH, while very strong bases might have a pH greater than 14.",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.VPM,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 0m,
                 },
                 new StopItemModel {
                   Index = 2,
                   Name = "ALK",
                   Description ="Alkalinity The buffering capacity of a water body; a measure of the ability of the water body to neutralize acids and bases and thus maintain a fairly stable pH leve",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.VPM,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 0m,
                 },
                  new StopItemModel {
                   Index = 3,
                   Name = "CAL",
                   Description ="Calcium contribute to the physiology and biochemistry of organisms cell",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.VPM,
                   Frequency = 3,
                   FrequencyType = Enums.eFrequencyType.Monthly,
                   Price = 0m,
                 },
                  new StopItemModel {
                   Index = 4,
                   Name = "CY",
                   Description ="Cyanuric Acid One of the most common chemicals used for the prevention of chlorine loss in swimming pools is Cyanuric Acid (also called CYA, Conditioner, or Stabilizer) it protects the free chlorine from being destroyed by the sun ultraviolet rays",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.VPM,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Monthly,
                   Price = 0m,
                 },
                 new StopItemModel {
                   Index = 5,
                   Name = "WT",
                   Description ="Water Temperature",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Temperature,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 0m,
                 },
                 new StopItemModel {
                   Index = 6,
                   Name = "L-CHLO",
                   Description ="Liquid bleach",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Gallon,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 20m,
                 },
                 new StopItemModel {
                   Index = 7,
                   Name = "Tbts",
                   Description ="Tablets Triclor",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Unit,
                   Price = 20m,
                 },
                 new StopItemModel {
                   Index = 8,
                   Name = "Skim",
                   Description ="Surface cleaner",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Device,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                   Price = 20m,
                 },
                 new StopItemModel {
                   Index = 9,
                   Name = "Pump",
                   Description ="Water pump",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Device,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                 },
                 new StopItemModel {
                   Index = 10,
                   Name = "Filter",
                   Description ="Pool filter",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Device,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Monthly,
                 },
                 new StopItemModel {
                   Index = 11,
                   Name = "H2o",
                   Description ="Water check",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Gallon,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Monthly,
                 },
                 new StopItemModel {
                   Index = 12,
                   Name = "Vac-p",
                   Description ="Vacuum pool",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Device,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                 },
                 new StopItemModel {
                   Index = 13,
                   Name = "#S-Alk",
                   Description ="Scoop",
                   ItemType = Enums.eItemType.Stop,
                   VolumeType = Enums.eVolumeType.Device,
                   IsCheckField = true,
                   Frequency = 1,
                   FrequencyType = Enums.eFrequencyType.Weekly,
                 }
                };

                    await stopItemController.InsertAll(items);
                    SimpleIoc.Default.GetInstance<IUserDialogs>().Toast($"{items.Count} Items successfully generated", TimeSpan.FromSeconds(3));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                SimpleIoc.Default.GetInstance<IUserDialogs>().Toast(e.Message, TimeSpan.FromSeconds(5));
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
                    DependencyService.Get<IUserDialogs>().Toast("Geocode Address, no internet connectivity is available now, check airplain mode if apply your addres is not be setted to geocode.", TimeSpan.FromSeconds(5));
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
        /// Get device current address geocode position
        /// </summary>
        /// <param name="fullAddress"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> GetDeviceAddressAsync()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.None)
                {
                    DependencyService.Get<IUserDialogs>().Toast("Geocode Address, no internet connectivity is available now, check airplain mode if apply your addres is not be setted to geocode.", TimeSpan.FromSeconds(5));
                    return null;
                }

                var device = await GetPositionAsync();

                if (device != null)
                {
                    Geocoder geoCoder = new Geocoder();
                    var approximateLocations = await geoCoder.GetAddressesForPositionAsync(new Position(device.Latitude, device.Longitude));
                    return approximateLocations;
                }

                return null;
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
                || (selectedCustomers != null && !selectedCustomers.Any()))
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

        /// <summary>
        /// Convert Volumen unit type
        /// </summary>
        /// <param name="units"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double ConvertUnits(this int units, eVolumeType from, eVolumeType to)
        {
            double[][] factor =
                {
                new double[] {1, 2, 0.25},
                new double[] {0.5, 1, 0.125},
                new double[] {4, 8, 1}
            };

            return units * factor[(int)from][(int)to];
        }

        /// <summary>
        /// Convert mass unit type
        /// </summary>
        /// <param name="units"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double ConvertUnits(this double units, eMassUnit from, eMassUnit to)
        {
            double results = units;

            switch (from)
            {
                case eMassUnit.Grams:
                    switch (to)
                    {
                        case eMassUnit.Grams:
                            break;
                        case eMassUnit.Kilograms:
                            results = units * 0.001;
                            break;
                        case eMassUnit.Ounces:
                            results = units * 0.035274;
                            break;
                        case eMassUnit.Pounds:
                            results = units * 0.00220462;
                            break;
                        default:
                            break;
                    }
                    break;
                case eMassUnit.Kilograms:
                    switch (to)
                    {
                        case eMassUnit.Grams:
                            results = units * 1000;
                            break;
                        case eMassUnit.Kilograms:
                            break;
                        case eMassUnit.Ounces:
                            results = units * 35.274;
                            break;
                        case eMassUnit.Pounds:
                            results = units * 2.20462;
                            break;
                        default:
                            break;
                    }
                    break;
                case eMassUnit.Ounces:
                    switch (to)
                    {
                        case eMassUnit.Grams:
                            results = units * 28.3495;
                            break;
                        case eMassUnit.Kilograms:
                            results = units * 0.0283495;
                            break;
                        case eMassUnit.Ounces:
                            break;
                        case eMassUnit.Pounds:
                            results = units * 0.0625;
                            break;
                        default:
                            break;
                    }
                    break;
                case eMassUnit.Pounds:
                    switch (to)
                    {
                        case eMassUnit.Grams:
                            results = units * 453.592;
                            break;
                        case eMassUnit.Kilograms:
                            results = units * 0.453592;
                            break;
                        case eMassUnit.Ounces:
                            results = units * 16;
                            break;
                        case eMassUnit.Pounds:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }


            return results;
        }
    }
}