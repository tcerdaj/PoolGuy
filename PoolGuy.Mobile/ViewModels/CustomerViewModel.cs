
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
using PoolGuy.Mobile.Views;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Specialized;
using Xamarin.Forms.Internals;
using CommonServiceLocator;

namespace PoolGuy.Mobile.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        Page _page;
        public CustomerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
        }

        public void InitPages()
        {
            Pages = new List<CustomerPageViewModel> {
              new CustomerPageViewModel { Title = "Customer", Page =  new WCustomerPage()},
              new CustomerPageViewModel { Title = "Address", Page =  new WAddressPage()},
              new CustomerPageViewModel { Title = "Contact", Page =  new WContactPage()},
              new CustomerPageViewModel { Title = "Pool", Page =  new WPoolPage()},
            };
        }

        public double Progress
        {
            get => FieldsCompleted / Fields;
        }

        public int Percent
        {
            get => (int)Progress * 100;
        }

        public int Fields
        {
            get
            {
                var count = (from c in typeof(CustomerModel).GetProperties()
                             where c.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0 ||
                                   c.GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0 ||
                                   c.GetCustomAttributes(typeof(MaxLengthAttribute), false).Count() > 0
                             select c).Count() +
                 (from c in typeof(AddressModel).GetProperties()
                  where c.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0 ||
                       c.GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0 ||
                       c.GetCustomAttributes(typeof(MaxLengthAttribute), false).Count() > 0
                  select c).Count() +
                 (from c in typeof(ContactInformationModel).GetProperties()
                  where c.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0 ||
                       c.GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0 ||
                       c.GetCustomAttributes(typeof(MaxLengthAttribute), false).Count() > 0
                  select c).Count() +
                 (from c in typeof(PoolModel).GetProperties()
                  where c.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0 ||
                       c.GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0 ||
                       c.GetCustomAttributes(typeof(MaxLengthAttribute), false).Count() > 0
                  select c).Count();

                return count;
            }
        }

        Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                Dictionary<string, PropertyInfo> result = new Dictionary<string, PropertyInfo>();

                (from c in typeof(CustomerModel).GetProperties()
                 where c.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0 ||
                       c.GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0 ||
                       c.GetCustomAttributes(typeof(MaxLengthAttribute), false).Count() > 0
                 select new KeyValuePair<string, PropertyInfo>(c.Name, c)).ToDictionary(x => x.Key, y => y.Value).ForEach((e) =>
                 {
                     result.Add(e.Key, e.Value);

                 });

                (from c in typeof(AddressModel).GetProperties()
                 where c.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0 ||
                      c.GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0 ||
                      c.GetCustomAttributes(typeof(MaxLengthAttribute), false).Count() > 0
                 select new KeyValuePair<string, PropertyInfo>(c.Name, c)).ToDictionary(x => x.Key, y => y.Value).ForEach((e) =>
                 {
                     result.Add(e.Key, e.Value);

                 });

                (from c in typeof(ContactInformationModel).GetProperties()
                 where c.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0 ||
                      c.GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0 ||
                      c.GetCustomAttributes(typeof(MaxLengthAttribute), false).Count() > 0
                 select new KeyValuePair<string, PropertyInfo>(c.Name, c)).ToDictionary(x => x.Key, y => y.Value).ForEach((e) =>
                 {
                     result.Add(e.Key, e.Value);

                 });

                (from c in typeof(PoolModel).GetProperties()
                 where c.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0 ||
                      c.GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0 ||
                      c.GetCustomAttributes(typeof(MaxLengthAttribute), false).Count() > 0
                 select new KeyValuePair<string, PropertyInfo>(c.Name, c)).ToDictionary(x => x.Key, y => y.Value).ForEach((e) =>
                 {
                     result.Add(e.Key, e.Value);

                 });

                return result;
            }
        }

        public int FieldsCompleted
        {
            get
            {
                int count = 0;

                Type type = Customer.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                foreach (PropertyInfo prop in props)
                {
                    if (Properties.ContainsValue(prop))
                    {
                        object propValue = prop.GetValue(Customer, null);
                        if (propValue != null)
                        {
                            count++;
                        }
                    }
                }

                type = Address.GetType();
                props = new List<PropertyInfo>(type.GetProperties());
                foreach (PropertyInfo prop in props)
                {
                    if (Properties.ContainsValue(prop))
                    {
                        object propValue = prop.GetValue(Address, null);
                        if (propValue != null)
                        {
                            count++;
                        }
                    }
                }

                type = Contact.GetType();
                props = new List<PropertyInfo>(type.GetProperties());
                foreach (PropertyInfo prop in props)
                {
                    if (Properties.ContainsValue(prop))
                    {
                        object propValue = prop.GetValue(Contact, null);
                        if (propValue != null)
                        {
                            count++;
                        }
                    }
                }

                type = Pool.GetType();
                props = new List<PropertyInfo>(type.GetProperties());
                foreach (PropertyInfo prop in props)
                {
                    if (Properties.ContainsValue(prop))
                    {
                        object propValue = prop.GetValue(Pool, null);
                        if (propValue != null)
                        {
                            count++;
                        }
                    }
                }

                return count;
            }
        }

        private CustomerModel _customer = new CustomerModel() { };
        public CustomerModel Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private AddressModel _address = new AddressModel();
        public AddressModel Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged("Address"); }
        }

        private ContactInformationModel _contact = new ContactInformationModel();
        public ContactInformationModel Contact
        {
            get { return _contact; }
            set { _contact = value; OnPropertyChanged("Contact"); }
        }

        private PoolModel _pool = new PoolModel();
        public PoolModel Pool
        {
            get { return _pool; }
            set { _pool = value; OnPropertyChanged("Pool"); }
        }

        public string[] PoolTypes
        {
            get { return Enum.GetNames(typeof(PoolType)); }
        }

        private string errorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged("ErrorMessage"); OnPropertyChanged("ErrorTextColor"); }
        }

        public string ErrorTextColor
        {
            get { return ErrorMessage.Contains("Unable") || ErrorMessage.Contains("Error") ? "Red" : "#009d00"; }
        }

        private List<CustomerPageViewModel> _pages;

        public List<CustomerPageViewModel> Pages
        {
            get { return _pages; }
            set { _pages = value; OnPropertyChanged("Pages"); }
        }

        private int _position = 0;

        public int Position
        {
            get { return _position; }
            set { _position = value; OnPropertyChanged("Position"); }
        }

        public ICommand GoToPageCommand
        {
            get { return new RelayCommand<string>((page) => GoToPage(page)); }
        }

        private void GoToPage(string page)
        {
            switch (page)
            {
                case "Customer":
                    if (Position != 0)
                    {
                        Position = 0;
                    }
                    break;
                case "Address":
                    if (Position != 1)
                    {
                        Position = 1;
                    }
                    break;
                case "Contact":
                    if (Position != 2)
                    {
                        Position = 2;
                    }
                    break;
                case "Pool":
                    if (Position != 3)
                    {
                        Position = 3;
                    }
                    break;

                default:
                    break;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(async () => await SaveCustomerAsync());
            }
        }

        private async Task SaveCustomerAsync()
        {
            try
            {
                if (!FieldValidationHelper.IsFormValid(Customer, _page))
                {
                    ErrorMessage = "Unable to save customer";
                    return;
                }

                ErrorMessage = "";

                Geocoder geoCoder = new Geocoder();
                IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync($"{Customer.Address.Address1}, {Customer.Address.City}, {Customer.Address.State} {Customer.Address.Zip}");
                Position position = approximateLocations.FirstOrDefault();
                Customer.Latitude = position.Latitude;
                Customer.Longitude = position.Longitude;

                var customerController = new CustomerController();
                var result = await customerController.ModifyAsync(Customer);

                ErrorMessage = result?.Status == Enums.eResultStatus.Ok ? "Save Customer Success" :
                               $"Unable to save customer: {result?.Message}";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                ErrorMessage = $"Error: {e.Message}";
            }
        }

        public bool IsValid()
        {
            bool isValid = false;
            switch (Title)
            {
                case "Customer":
                    var page = Pages.FirstOrDefault(x => x.Title == Title);
                    if (page == null)
                    {
                        return false;
                    }

                    isValid = FieldValidationHelper.IsFormValid(page.Customer, page.Page);
                    break;
                default:
                    break;
            }

            return isValid;
        }

        public void OnCustomerChanged()
        {
            OnPropertyChanged("Customer");
        }

        public void OnPoolChanged()
        {
            OnPropertyChanged("Customer");
        }
    }
}