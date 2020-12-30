using PoolGuy.Mobile.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.Data.Models.Config;
using PoolGuy.Mobile.Extensions;
using Xamarin.Forms.GoogleMaps;
using PoolGuy.Mobile.Data.Controllers;
using Newtonsoft.Json;

namespace PoolGuy.Mobile.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public MapViewModel()
        {
            if (Globals.CurrentPage != Enums.ePage.Map)
            {
                Globals.CurrentPage = Enums.ePage.Map;
            }

            Title = this.GetType().Name.Replace("ViewModel", "").Replace("Search", "");
        }

        private string _routeSummary;

        public string RouteSummary
        {
            get { return _routeSummary; }
            set { _routeSummary = value; OnPropertyChanged("RouteSummary"); }
        }

        private List<CustomerModel> _customers = new List<CustomerModel>();
        public List<CustomerModel> Customers
        {
            get { return _customers; }
            set { _customers = value; OnPropertyChanged("Customers"); }
        }

        public Page CurrentPage
        {
            get => NavigationService.CurrentPage;
        }

        public async Task InitializeAsync()
        {
            if(IsBusy){ return; }
            IsBusy = true;

            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (CurrentPage.FindByName("map") is Map map
                        && Customers.Any())
                    {
                       
                        // Add route poligon
                        var route = new Polyline
                        {
                            StrokeColor = Color.FromHex("#FF9900"),
                            StrokeWidth = 5,
                        };

                        var allPoints = Customers
                            .Select(x => new Position(x.Latitude, x.Longitude))
                            .ToList();

                        var mapSpan = allPoints.FromPositions();

                         map.MoveToRegion(mapSpan);

                        var startPoint = $"{Globals.BranchLocation.Latitude},{Globals.BranchLocation.Longitude}";
                        var endPoint = $"{allPoints.LastOrDefault().Latitude},{allPoints.LastOrDefault().Longitude}";
                        var stops = string.Join("|", allPoints.Take(allPoints.Count - 1).Select(x => $"{x.Latitude},{x.Longitude}").ToArray());

                        var directionHistory = await new DirectionController().LocalData.List();
                        var direction = directionHistory.LastOrDefault() ?? new Data.Models.GoogleMap.DirectionHistory();
                        var schedulers = await new SchedulerController().LocalData.List();
                        var scheLastModification = schedulers.Min(x => x.Modified) ?? schedulers.Min(x => x.Created);

                        if (direction.Direction == null || (direction.Direction != null
                            && direction.Modified.HasValue
                            && (direction.Modified.Value.ToLocalTime() - DateTime.Now).Days >
                            (!scheLastModification.HasValue ? 0 : (scheLastModification.Value.ToLocalTime() - DateTime.Now).Days)))
                        {

                            var mapService = DependencyService
                            .Get<IGoogleMapService>();

                            var directionServer = await mapService
                            .GetDirections(startPoint.ToString(),
                            endPoint.ToString(),
                            stops,
                            Config.ApiKeys.FirstOrDefault(x => x.ConsumerKey == "GoogleMap").ConsumerSecret);

                            if (directionServer.status != "OK")
                            {
                                return;
                            }

                            direction.Json = JsonConvert.SerializeObject(directionServer);
                            await new DirectionController().LocalData.Modify(direction);
                        }

                        // Draw route map
                        var points = direction.Direction.routes.FirstOrDefault().overview_polyline.points.DecodePolyline();
                        points.ForEach((p) =>
                        {
                            route.Positions.Add(p);
                        });

                        // Add pins with label name, address
                        int totalDistance = 0;
                        int totalTime = 0;
                        foreach (var c in Customers)
                        {
                            try
                            {
                                var index = Customers.IndexOf(c);
                                var leg = direction.Direction.routes.FirstOrDefault()
                                .legs[index];
                                
                                totalDistance += leg.distance.value;
                                totalTime += leg.duration.value;

                                Pin customerPin = new Pin
                                {
                                    Position = new Position(c.Latitude, c.Longitude),
                                    Label = $"{c.Index + 1}-{c.Name}",
                                    Address = $"{c.Address.Address1} | {leg.distance.text}, {leg.duration.text}",
                                    Type = PinType.Place,
                                    Icon = index == 0
                                    ? BitmapDescriptorFactory.DefaultMarker(Color.Aqua)
                                    : (index + 1) == Customers.Count
                                    ? BitmapDescriptorFactory.DefaultMarker(Color.Violet)
                                    : BitmapDescriptorFactory.DefaultMarker(Color.Red)
                                };

                                // Add pin to map
                                map.Pins.Add(customerPin);
                            }
                            catch (Exception e)
                            {
                                throw;
                            }
                        }

                        // Add route to map
                        map.Polylines.Add(route);
                        
                        TimeSpan t = TimeSpan.FromSeconds(totalTime);
                        string hourMinutes = "";
                        decimal miles = Math.Round((decimal)(totalDistance * 0.000621371192), 2);
                        if (t.Hours > 0)
                        {
                            hourMinutes = string.Format("{0:D2}h:{1:D2}m",
                                        t.Hours,
                                        t.Minutes);
                        }
                        else 
                        {
                            hourMinutes = string.Format("{0:D2}m",
                                        t.Minutes);
                        }

                        RouteSummary = $"{Customers.Count} customers, {miles} mi, {hourMinutes}, {Math.Round(miles / 20,2)} gls";
                    }
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally { IsBusy = false; }
        }

        public ICommand GoBackCommand
        {
            get
            {
                return new RelayCommand(async () => {
                    await NavigationService.CloseModal();
                });
            }
        }
    }
}