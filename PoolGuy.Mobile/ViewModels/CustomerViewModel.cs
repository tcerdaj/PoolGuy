
using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using static PoolGuy.Mobile.Data.Models.Enums;
using PoolGuy.Mobile.Data.SQLite;

namespace PoolGuy.Mobile.ViewModels
{
    public class SearchCustomerViewModel : BaseViewModel
    {
        //private Page _page;
        //public CustomerViewModel(Page page)
        //{
        //    Title = this.GetType().Name.Replace("ViewModel", "");
        //    _page = page;
        //}

        //private CustomerModel _customer = new CustomerModel() {Pool = new PoolModel()};

        //public CustomerModel Customer
        //{
        //    get { return _customer; } 
        //    set { _customer = value; OnPropertyChanged("Customer"); }
        //}

        //private PoolModel _pool = new PoolModel();

        //public PoolModel Pool
        //{
        //    get { return _pool; }
        //    set { _pool = value; OnPropertyChanged("Pool"); }
        //}

        //public string[] PoolTypes
        //{
        //    get { return Enum.GetNames(typeof(PoolType)); }
        //}

        //private string errorMessage = string.Empty;

        //public string ErrorMessage
        //{
        //    get { return errorMessage; }
        //    set { errorMessage = value; OnPropertyChanged("ErrorMessage"); OnPropertyChanged("ErrorTextColor"); }
        //}

        //public string ErrorTextColor
        //{
        //    get { return ErrorMessage.Contains("Unable") || ErrorMessage.Contains("Error") ? "Red" : "#009d00"; }
        //}

        //public ICommand SaveCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(async ()=>await SaveCustomerAsync());
        //    }
        //}

        //private async Task SaveCustomerAsync()
        //{
        //    try
        //    {
        //        if (!FieldValidationHelper.IsFormValid(Customer, _page)) 
        //        {
        //            ErrorMessage = "Unable to save customer";
        //            return;
        //        }

        //        ErrorMessage = "";

        //        Geocoder geoCoder = new Geocoder();
        //        IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync($"{Customer.Address1}, {Customer.City}, {Customer.State} {Customer.Zip}");
        //        Position position = approximateLocations.FirstOrDefault();
        //        Customer.Latitude = position.Latitude;
        //        Customer.Longitude = position.Longitude;

        //        var customerController = new CustomerController();

        //        var customers = await customerController.LocalData.List(new Data.Models.Query.SQLControllerListCriteriaModel()
        //        {
        //            Filter = new List<Data.Models.Query.SQLControllerListFilterField>() { 
        //              new Data.Models.Query.SQLControllerListFilterField(){ FieldName = "FirstName", ValueLBound = Customer.FirstName},
        //              new Data.Models.Query.SQLControllerListFilterField(){ FieldName = "LastName", ValueLBound = Customer.LastName}
        //            }
        //        }).ConfigureAwait(false);

        //        var _customer = customers.FirstOrDefault();

        //        if (_customer != null)
        //        {
        //            Customer.Id = _customer.Id;
        //        }

        //        var result = await customerController.ModifyAsync(Customer);

        //        ErrorMessage = result?.Status == Enums.eResultStatus.Ok? "Save Customer Success" :
        //                       $"Unable to save customer: {result?.Message}";
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e);
        //        ErrorMessage = $"Error: {e.Message}";
        //    }
        //}

        //public void OnPoolChanged()
        //{
        //    OnPropertyChanged("Customer");
        //}
    }
}