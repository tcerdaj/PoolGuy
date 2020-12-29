using PoolGuy.Mobile.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms.Maps;
using System.Linq;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.Data.Models.Config;

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
                if (CurrentPage.FindByName("map") is Map map 
                    && Customers.Any())
                {
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Globals.BranchLocation.Latitude,
                        Globals.BranchLocation.Longitude), Distance.FromMiles(2)));
                    map.MapType = MapType.Street;
                    // Add route poligon
                    var route = new Polyline
                    {
                        StrokeColor = Color.FromHex("#FF9900"),
                        StrokeWidth = 12,
                    };

                    var allPoints = Customers
                        .Select(x => new Position(x.Latitude, x.Longitude))
                        .ToList();

                    var startPoint = allPoints.FirstOrDefault();
                    var endPoint = allPoints.LastOrDefault();
                    var stops = string.Join("|", allPoints.Where(x => x != startPoint && x != endPoint));

                    var direction = await DependencyService
                        .Get<IGoogleMapService>()
                        .GetDirections(startPoint.ToString(), 
                        endPoint.ToString(), 
                        stops, 
                        Config.ApiKeys.FirstOrDefault(x => x.ConsumerKey == "GoogleMap").ConsumerSecret);

                    // Add pins with label name, address
                    List<Pin> pins = new List<Pin>();
                    Customers.ForEach((c) => {
                        Pin customerPin = new Pin
                        {
                            Position = new Position(c.Latitude, c.Longitude),
                            Label = $"{c.Name}",
                            Address = c.Address.Address1,
                            Type = PinType.SavedPin,
                        };

                        customerPin.MarkerClicked += async (s, args) =>
                        {
                            args.HideInfoWindow = true;
                            var pin = (Pin)s;
                            await Message.DisplayAlertAsync(pin.Label, $"{pin.Address}.", "Ok");
                        };

                        // Add pin to map
                        map.Pins.Add(customerPin);
                    });
                    
                    // Add route to map
                    map.MapElements.Add(route);
                }
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