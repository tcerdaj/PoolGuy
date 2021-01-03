
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
using PoolGuy.Mobile.Views;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Xamarin.Forms.Internals;
using Omu.ValueInjecter;
using Newtonsoft.Json;
using PoolGuy.Mobile.Extensions;
using PoolGuy.Mobile.Services.Interface;
using System.Collections.ObjectModel;

namespace PoolGuy.Mobile.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        public CustomerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
            Globals.CurrentPage = Enums.ePage.Customer;
            SubscribeMessage();
        }

        private void SubscribeMessage()
        {
            Notify.SubscribeVisitingDayActionAction(async (sender) => {
                if (sender.Object is SchedulerModel scheduler)
                {
                    Pages[0].Customer.Scheduler?.Clear();

                    if (scheduler.Selected)
                    {
                        Pages[0].Customer.Scheduler.Add(scheduler);
                    }
                    
                    _wasModified = true;

                    Pages[0].NotifyPropertyChanged("Customer");
                }
                 
                Pages[1].Schedulers = await GetScheduler(Pages[0].Customer);
            });
        }

        public void InitPages(CustomerModel customer = null)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                ErrorMessage = "";
                OriginalCustomer = "";
                ObservableCollection<SchedulerModel> schedulers = await GetScheduler(customer);
                
                Pages = new List<CustomerPageViewModel> {
                  new CustomerPageViewModel { Title = "Customer", Page =  new WCustomerPage(this){ Title = "Customer"} },
                  new CustomerPageViewModel { Title = "Address", Page =  new WAddressPage(this){ Title = "Address" }, Schedulers = schedulers },
                  new CustomerPageViewModel { Title = "Contact", Page =  new WContactPage(this){ Title = "Contact" } },
                  new CustomerPageViewModel { Title = "Pool", Page =  new WPoolPage(this){ Title = "Pool" } },
                };

                if (customer != null)
                {
                    Customer = (CustomerModel)new CustomerModel().InjectFrom(customer);
                    OriginalCustomer = JsonConvert.SerializeObject(customer,
                                Formatting.Indented,
                                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    Pages[0].Customer = customer;
                    Pages[1].Address = customer.Address;
                    Pages[1].Schedulers = schedulers;
                    Pages[2].Contact = customer.Contact;
                    Pages[3].Pool = customer.Pool;
                }

                Position = 0;
            });
        }

        private async Task<ObservableCollection<SchedulerModel>> GetScheduler(CustomerModel customer)
        {
            var schedulers = new ObservableCollection<SchedulerModel>(await new SchedulerController()
                    .ListWithChildrenAsync(new Data.Models.Query.SQLControllerListCriteriaModel
                    {
                        Sort = new List<Data.Models.Query.SQLControllerListSortField> {
                        new Data.Models.Query.SQLControllerListSortField {
                          FieldName = "Index"
                        }
                     }
                    }));

            if (customer != null)
            {
                schedulers.Where(x => customer.Scheduler.Any(s => s.Id == x.Id)).ForEach((s) =>
                {
                    s.Selected = true;
                });
            }
            
            return schedulers;
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
                int count = 0;

                try
                {
                    count =
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
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

                return count;
            }
        }

        Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                Dictionary<string, PropertyInfo> result = new Dictionary<string, PropertyInfo>();

                try
                {
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
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

                return result;
            }
        }

        public int FieldsCompleted
        {
            get
            {
                int count = 0;

                try
                {
                    foreach (var page in Pages)
                    {
                        Type type;
                        IList<PropertyInfo> props;

                        switch (page.Title)
                        {
                            case "Customer":
                                if (page?.Customer != null)
                                {
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
                                }
                                break;
                            case "Address":
                                if (page?.Address != null)
                                {
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
                                }
                                break;
                            case "Contact":
                                if (page?.Contact != null)
                                {
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
                                }
                                break;
                            case "Pool":
                                if (page?.Pool != null)
                                {
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
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                   Debug.WriteLine(e);
                }

                return count;
            }
        }

        public string OriginalCustomer { get; set; }

        private CustomerModel _customer = new CustomerModel() { 
            Contact = new ContactModel(),
            Address = new AddressModel(),
            HomeAddress = new AddressModel(),
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

        private bool _wasModified = false;
        public bool WasModified 
        { 
            get 
            {

                try
                {
                    CustomerModel _originalModel = new CustomerModel()
                    {
                        Contact = new ContactModel(),
                        Address = new AddressModel(),
                        HomeAddress = new AddressModel(),
                        Pool = new PoolModel()
                    };

                    if (!string.IsNullOrEmpty(OriginalCustomer))
                    {
                        _originalModel = JsonConvert.DeserializeObject<CustomerModel>(OriginalCustomer);
                    }

                    return (_originalModel.DetailedCompare(Pages[0].Customer).Any() ||
                            _originalModel.HomeAddress.DetailedCompare(Pages[0].Customer.HomeAddress).Any() ||
                            _originalModel.Address.DetailedCompare(Pages[1].Address).Any() ||
                            _originalModel.Contact.DetailedCompare(Pages[2].Contact).Any() ||
                            _originalModel.Pool.DetailedCompare(Pages[3].Pool).Any()) 
                           || _wasModified;
                }
                catch (Exception)
                {
                    return true;
                }
            }
            set { _wasModified = value; }
        }

        public ICommand GoBackCommand => new RelayCommand(async () =>
        {
            if (WasModified)
            {
                if (!await Message.DisplayConfirmationAsync("Warning", "Are you sure to exit without saving the changes?", "Exit", "Cancel"))
                {
                    // Return to last page where you can save
                    Position = 3;
                    return;
                }
            }

            await NavigationService.CloseModal(false);
            Notify.RaiseSearchCustomerAction(new Messages.RefreshMessage());
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
                    if (Position != 0 && IsValid(Pages[Position]))
                    {
                        Position = 0;
                    }
                    break;
                case "Address":
                    if (Position != 1 && IsValid(Pages[Position]))
                    {
                        Position = 1;
                    }
                    break;
                case "Contact":
                    if (Position != 2 && IsValid(Pages[Position]))
                    {
                        Position = 2;
                    }
                    break;
                case "Pool":
                    if (Position != 3 && IsValid(Pages[Position]))
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

                Position? position = await Customer.Address.FullAddress.GetPositionAsync();
                if (position.HasValue)
                {
                    Customer.Latitude = position.Value.Latitude;
                    Customer.Longitude = position.Value.Longitude;
                }

                var customerController = new CustomerController();
                await customerController.ModifyWithChildrenAsync(Customer);
                OnPropertyChanged("Progress");
                OnPropertyChanged("Percent");
                Pages[3].NotifyPropertyChanged("ShowAddEquipment");
                WasModified = false;
                ErrorMessage = "Save Customer Success";
                OriginalCustomer = JsonConvert.SerializeObject(Customer,
                            Formatting.Indented,
                            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                ErrorMessage = $"Error: {e.Message}";
            }
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

        public ICommand TakePhotoCommand
        {
            get => new RelayCommand(async () => await TakePhotoAsync());
        }

        private async Task TakePhotoAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var action = await Message.DisplayActionSheetAsync("Select Image Source", "Cancel", 
                    "Gallery", "Camera");
                if (string.IsNullOrEmpty(action) || action == "Cancel")
                {
                    return;
                }

                var imageService = DependencyService.Get<IImageService>();
                var photo = await imageService.TakePhoto(action);

                if (photo == null)
                {
                    return;
                }

                Pages[0].Customer.ImageUrl = photo.Path;
                Pages[0].Customer.NotififyImageUrl();
            }
            catch (Exception e)
            {
                await Message.DisplayAlertAsync(e.Message, Title, "Ok");
            }
            finally { IsBusy = false; }
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
                     f.GetGetMethod().ReturnType == typeof(Guid?) ||
                    f.GetGetMethod().ReturnType == typeof(int) ||
                    f.GetGetMethod().ReturnType == typeof(int?) ||
                    f.GetGetMethod().ReturnType == typeof(double) ||
                    f.GetGetMethod().ReturnType == typeof(double?) ||
                    f.GetGetMethod().ReturnType == typeof(decimal?) ||
                    f.GetGetMethod().ReturnType == typeof(decimal) ||
                    f.GetGetMethod().ReturnType.FullName.Contains("Enums") ||
                    f.GetGetMethod().ReturnType == typeof(DateTime) ||
                    f.GetGetMethod().ReturnType == typeof(DateTime?) ||
                    f.GetGetMethod().ReturnType == typeof(Boolean) ||
                    f.GetGetMethod().ReturnType == typeof(Boolean?))
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