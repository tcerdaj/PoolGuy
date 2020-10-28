
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
using Xamarin.Forms.Internals;
using Omu.ValueInjecter;

namespace PoolGuy.Mobile.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        public CustomerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
        }

        public void InitPages(CustomerModel customer = null)
        {
            ErrorMessage = "";
            Position = 0;

            Pages = new List<CustomerPageViewModel> {
              new CustomerPageViewModel { Title = "Customer", Page =  new WCustomerPage() },
              new CustomerPageViewModel { Title = "Address", Page =  new WAddressPage()},
              new CustomerPageViewModel { Title = "Contact", Page =  new WContactPage()},
              new CustomerPageViewModel { Title = "Pool", Page =  new WPoolPage()},
            };

            if(customer != null)
            {
                Customer = (CustomerModel)new CustomerModel().InjectFrom(customer);
                Pages[0].Customer = customer;
                Pages[1].Address = customer.Address;
                Pages[2].Contact = customer.Contact;
                Pages[3].Pool = customer.Pool;
            }
        }

        public double Progress => (double)FieldsCompleted / (double)Fields;

        public double Percent
        {
            get
            {
                var percent = (double)Progress * 100;
                return percent;
            }
        }

        public int Fields
        {
            get
            {
                var count =
                    (from c in typeof(CustomerModel).GetProperties()
                     where c.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0 ||
                     c.GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0 ||
                     c.GetCustomAttributes(typeof(MaxLengthAttribute), false).Count() > 0
                     select c).Count() +
                 (from c in typeof(AddressModel).GetProperties()
                  where c.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0 ||
                       c.GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0 ||
                       c.GetCustomAttributes(typeof(MaxLengthAttribute), false).Count() > 0
                  select c).Count() +
                 (from c in typeof(ContactModel).GetProperties()
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

                (from c in typeof(ContactModel).GetProperties()
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

                foreach (var page in Pages)
                {
                    Type type;
                    IList<PropertyInfo> props;

                    switch (page.Title)
                    {
                        case "Customer":
                            type = page.Customer.GetType();
                            props = new List<PropertyInfo>(type.GetProperties());
                            foreach (PropertyInfo prop in props)
                            {
                                if (Properties.ContainsValue(prop))
                                {
                                    object propValue = prop.GetValue(page.Customer, null);
                                    if (propValue != null)
                                    {
                                        count++;
                                    }
                                }
                            }
                            break;
                        case "Address":
                            type = page.Address.GetType();
                            props = new List<PropertyInfo>(type.GetProperties());
                            foreach (PropertyInfo prop in props)
                            {
                                if (Properties.ContainsValue(prop))
                                {
                                    object propValue = prop.GetValue(page.Address, null);
                                    if (propValue != null)
                                    {
                                        count++;
                                    }
                                }
                            }
                            break;
                        case "Contact":
                            type = page.Contact.GetType();
                            props = new List<PropertyInfo>(type.GetProperties());
                            foreach (PropertyInfo prop in props)
                            {
                                if (Properties.ContainsValue(prop))
                                {
                                    object propValue = prop.GetValue(page.Contact, null);
                                    if (propValue != null)
                                    {
                                        count++;
                                    }
                                }
                            }
                            break;
                        case "Pool":
                            type = page.Pool.GetType();
                            props = new List<PropertyInfo>(type.GetProperties());
                            foreach (PropertyInfo prop in props)
                            {
                                if (Properties.ContainsValue(prop))
                                {
                                    object propValue = prop.GetValue(page.Pool, null);
                                    if (propValue != null)
                                    {
                                        count++;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                return count;
            }
        }

        private CustomerModel _customer = new CustomerModel() { 
            Contact = new ContactModel(),
            Address = new AddressModel(),
            Pool = new PoolModel()
        };
        public CustomerModel Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
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
            set
            {
                _position = value;
                OnPropertyChanged("Position");
                OnPropertyChanged("Progress");
                OnPropertyChanged("Percent");
                OnPropertyChanged("IsVisibleName");
            }
        }

        public bool IsVisibleName
        {
            get { return Position > 0; }
        }

        public bool WasModified 
        { 
            get 
            {

                try
                {
                    return Customer.DetailedCompare(Pages[0].Customer).Any() ||
                                  Customer.Address.DetailedCompare(Pages[1].Address).Any() ||
                                  Customer.Contact.DetailedCompare(Pages[2].Contact).Any() ||
                                  Customer.Pool.DetailedCompare(Pages[3].Pool).Any();
                }
                catch (Exception)
                {
                    return true;
                }
            } 
        }

        public ICommand GoBackCommand => new RelayCommand(async () =>
        {
            if (WasModified)
            {
                if (!await Shell.Current.DisplayAlert("Warning", "Are you sure to exit without saving the changes?", "Exit", "Cancel"))
                {
                    return;
                }
            }

            await NavigationService.PopPopupAsync(false);
            Shell.Current.SendBackButtonPressed();
        });

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
                if (!WasModified)
                {
                    GoBackCommand.Execute(null);
                    return;
                }
                
                if (!Pages.Any(p => IsValid(p)))
                {
                    ErrorMessage = "Unable to save customer";
                    return;
                }

                ErrorMessage = "";
                Customer = Pages[0].Customer;
                Customer.Address = Pages[1].Address;
                Customer.Contact = Pages[2].Contact;
                Customer.Pool = Pages[3].Pool;

                Position position = await GetPosition(Customer.Address.FullAddress);
                Customer.Latitude = position.Latitude;
                Customer.Longitude = position.Longitude;

                var customerController = new CustomerController();
                await customerController.ModifyWithChildrenAsync(Customer);
                OnPropertyChanged("Progress");
                OnPropertyChanged("Percent");
                ErrorMessage = "Save Customer Success";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                ErrorMessage = $"Error: {e.Message}";
            }
        }

        private async Task<Position> GetPosition(string fullAddress)
        {
            Geocoder geoCoder = new Geocoder();
            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(fullAddress);
            Position position = approximateLocations.FirstOrDefault();
            return position;
        }

        public bool IsValid(CustomerPageViewModel page)
        {
            try
            {
                bool isValid = false;
                var controller = new CustomerController();

                switch (page.Title)
                {
                    case "Customer":
                        isValid = FieldValidationHelper.IsFormValid(page.Customer, page.Page);
                        break;
                    case "Address":
                        isValid = FieldValidationHelper.IsFormValid(page.Address, page.Page);
                        break;
                    case "Contact":
                        isValid = FieldValidationHelper.IsFormValid(page.Contact, page.Page);
                        break;
                    case "Pool":
                        isValid = FieldValidationHelper.IsFormValid(page.Pool, page.Page);
                        break;
                    default:
                        break;
                }

                return isValid;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
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
    static class extentions
    {
        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            List<Variance> variances = new List<Variance>();
            PropertyInfo[] fi = val1.GetType().GetProperties();
            foreach (PropertyInfo f in fi)
            {
                if (f.GetGetMethod().ReturnType == typeof(string) ||
                    f.GetGetMethod().ReturnType == typeof(Guid) ||
                    f.GetGetMethod().ReturnType == typeof(int) ||
                    f.GetGetMethod().ReturnType == typeof(double) ||
                    f.GetGetMethod().ReturnType == typeof(decimal) ||
                    f.GetGetMethod().ReturnType == typeof(Enum) ||
                    f.GetGetMethod().ReturnType == typeof(DateTime))
                {
                    Variance v = new Variance();
                    v.Prop = f.Name;
                    v.valA = f.GetValue(val1);
                    v.valB = f.GetValue(val2);
                    if (v.valA != null && !v.valA.Equals(v.valB))
                        variances.Add(v);
                }

            }
            return variances;
        }


    }
    class Variance
    {
        public string Prop { get; set; }
        public object valA { get; set; }
        public object valB { get; set; }
    }
}