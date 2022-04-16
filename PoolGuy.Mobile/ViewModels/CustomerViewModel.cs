
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
using Xamarin.Forms.Internals;
using Omu.ValueInjecter;
using Newtonsoft.Json;
using PoolGuy.Mobile.Extensions;
using PoolGuy.Mobile.Services.Interface;
using System.Collections.ObjectModel;
using PoolGuy.Mobile.Data.Models.Query;
using static PoolGuy.Mobile.Data.Models.Enums;
using System.Xml.Serialization;

namespace PoolGuy.Mobile.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        public CustomerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
            Globals.CurrentPage = Enums.ePage.SearchCustomer;
            SubscribeMessage();
            IsBusy = true;
            userDialogs = DependencyService.Get<IUserDialogs>();
            SubscribeMessages();
        }

        private void SubscribeMessages()
        {
            Notify.SubscribePoolAction(async (sender) => {
                try
                {
                    if (sender.Object is PoolModel pool)
                    {

                        Customer.Pool.Equipments = pool.Equipments;
                        Scheduler = await GetScheduler(Customer);
                        IsEditing = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
                }
            });

            Notify.SubscribeVisitingDayActionAction(async (sender) => {
                try
                {
                    Scheduler = await GetScheduler(Customer);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
                }
            });
        }
        #region Properties
        IUserDialogs userDialogs;

        public string OriginalCustomer { get; set; }
        private CustomerModel _customer = new CustomerModel() { 
            Contact = new ContactModel(),
            Address = new AddressModel(),
            HomeAddress = new AddressModel(),
            Pool = new PoolModel() { Type = PoolType.None },
            SameHomeAddress = false
        };

        [XmlIgnore]
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

        [XmlIgnore]
        public string ErrorTextColor
        {
            get { return ErrorMessage.Contains("Unable") || ErrorMessage.Contains("Error") ? "Red" : "#009d00"; }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get { return _isEditing; }
            set { _isEditing = value; OnPropertyChanged("IsEditing"); }
        }


        private bool _poolExpanded;
        public bool PoolExpanded
        {
            get { return _poolExpanded; }
            set { _poolExpanded = value; OnPropertyChanged("PoolExpanded"); }
        }

        [XmlIgnore]
        public List<PoolType> PoolTypes => new List<PoolType> { Enums.PoolType.None, Enums.PoolType.SweetPool, Enums.PoolType.SaltPool };

        [XmlIgnore]
        public CustomerPage Page { get; set; }


        ObservableCollection<SchedulerModel> _scheduler = new ObservableCollection<SchedulerModel>();
        public ObservableCollection<SchedulerModel> Scheduler 
        {
            get { return _scheduler; }
            set { _scheduler = value; OnPropertyChanged(nameof(Scheduler)); } 
        }

        private bool _useDeviceLocation;
        public bool UseDeviceLocation
        {
            get { return _useDeviceLocation; }
            set { _useDeviceLocation = value;OnPropertyChanged(nameof(UseDeviceLocation)); }
        }
        #endregion

        #region Commands


        public ICommand GoBackCommand => new RelayCommand(async () =>
        {
            if (IsEditing)
            {
                if (!await Message.DisplayConfirmationAsync("Warning", "Are you sure to exit without saving the changes?", "Exit", "Cancel"))
                {
                    return;
                }
            }

            await NavigationService.CloseModal(false);
            Notify.RaiseCustomerAction(new Messages.RefreshMessage());
        });

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(async () => await SaveCustomerAsync());
            }
        }

        public ICommand TogglePoolExpandedCommand
        {
            get
            {
                return new RelayCommand( () => PoolExpanded = !PoolExpanded);
            }
        }

        private async Task SaveCustomerAsync()
        {
            if (IsBusy)
            {
                return;
            }

            if (!IsEditing)
            {
                GoBackCommand.Execute(null);
                return;
            }

            try
            {
                var validationResult = IsValid();

                if (!validationResult.Key)
                {
                    Message.Toast($"Unable to save Customer. {validationResult.Value}", TimeSpan.FromSeconds(5));
                    return;
                }

                IsBusy = true;

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
                OnPropertyChanged("ShowAddEquipment");

                ErrorMessage = "Save Customer Success";
                OriginalCustomer = JsonConvert.SerializeObject(Customer,
                            Formatting.Indented,
                            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

                IsEditing = false;

                GoBackCommand.Execute(null);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Message.Toast($"Unable to save customer. Error details: {e.Message}", TimeSpan.FromSeconds(5));
            }
            finally
            {
                IsBusy = false;
            }
        }

        public KeyValuePair<bool, string> IsValid()
        {
            try
            {
                var result = new List<KeyValuePair<bool, string>>();

                result.Add(FieldValidationHelper.IsFormValid(Customer, Page));
                result.Add(FieldValidationHelper.IsFormValid(Customer.Address, Page));
                result.Add(FieldValidationHelper.IsFormValid(Customer.HomeAddress, Page));
                result.Add(FieldValidationHelper.IsFormValid(Customer.Pool, Page));
                result.Add(FieldValidationHelper.IsFormValid(Customer.Contact, Page));

                var success = !result.Any(x => x.Key == false);

                if (success)
                {
                    return new KeyValuePair<bool, string>(success, null);
                }

                var error = string.Join(",", result.Where(x => x.Key == false).Select(k => k.Value).ToArray<string>());

                return new KeyValuePair<bool, string>(success, error);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new KeyValuePair<bool, string>(false, e.Message);
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
            get => new RelayCommand(async () => await TakePoolPhotoAsync());
        }

        private async Task TakePoolPhotoAsync()
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

                var image = new EntityImageModel
                {
                    EntityId = Customer.Pool.Id,
                    ImageType = ImageType.Pool,
                    ImageUrl = photo.Path
                };

                Customer.Pool.Images.Add(image);
                OnPropertyChanged("Customer");
                IsEditing = true;
            }
            catch (Exception e)
            {
                await Message.DisplayAlertAsync(e.Message, Title, "Ok");
            }
            finally 
            { 
                IsBusy = false; 
            }
        }

        public ICommand ToggleSameAddressCommand 
        {
            get => new RelayCommand(() => ToggleSameAddress());
        }

        private void ToggleSameAddress()
        {
            Customer.SameHomeAddress = !Customer.SameHomeAddress;

            MakeBillingAddressSameHomeAddress();
        }

        public ICommand DeleteImageCommand
        {
            get => new RelayCommand<EntityImageModel>(async (img) => await DeleteImageAsync(img));
        }

        private async Task DeleteImageAsync(EntityImageModel img)
        {
            if (IsBusy || img == null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                if (!await userDialogs.DisplayConfirmationAsync("Delete Confirmation", "Are you sure want to delete image?", "Delete", "Cancel"))
                {
                    return;
                }

                await new ImageController().LocalData.Delete(img.Id);
                Customer.Pool.Images.Remove(img);
                OnPropertyChanged("Customer");
                IsEditing = true;
            }
            catch (Exception e)
            {
                await Message.DisplayAlertAsync(e.Message, Title, "Ok");
            }
            finally { IsBusy = false; }
        }

        public ICommand SelectEquipmentCommand
        {
            get => new RelayCommand<EquipmentModel>(async (model) => SelecteEquipment(model));
        }

        private async void SelecteEquipment(EquipmentModel model)
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                await NavigationService.NavigateToDialog(new EquipmentPage(model));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand DeleteEquipmentCommand
        {
            get => new RelayCommand<EquipmentModel>(async (model) => DeleteEquipment(model));
        }

        private async void DeleteEquipment(EquipmentModel model)
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                if (!await userDialogs.DisplayConfirmationAsync("Delete Confirmation", "Are you sure want to delete equipment?", "Delete", "Cancel"))
                {
                    return;
                }

                var obj = Customer.Pool.Equipments.FirstOrDefault(x => x.Id == model.Id);
                Customer.Pool.Equipments.Remove(obj);
                await new PoolController().ModifyWithChildrenAsync(Customer.Pool);
                Customer.Pool.RaiseEquipmentNotification();
                IsEditing = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand GoToAddEquipmentCommand
        {
            get => new RelayCommand(() => GoToAddEquipment());
        }

        private async void GoToAddEquipment()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                await NavigationService.NavigateToDialog(new EquipmentPage(new EquipmentModel { PoolId = Customer.Pool.Id, Pool = Customer.Pool })
                {
                    Title = "Select Equipment"
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand SelectImageCommand
        {
            get => new RelayCommand<EntityImageModel>(async (img) => await SelectImageAsync(img));
        }

        private async Task SelectImageAsync(EntityImageModel img)
        {
            if (IsBusy || img == null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                if (string.IsNullOrEmpty(img.ImageUrl))
                {
                    return;
                }

                await DependencyService.Get<IImageService>().DisplayImage(img.ImageUrl);
            }
            catch (Exception e)
            {
                await Message.DisplayAlertAsync(e.Message, Title, "Ok");
            }
            finally { IsBusy = false; }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    GoBackCommand.Execute(null);
                });
            }
        }

        public ICommand ToggleDeviceAddress
        {
            get {
                return new RelayCommand(async () =>
                {
                    if (IsBusy)
                    {
                        return;
                    }

                    UseDeviceLocation = !UseDeviceLocation;

                    if (UseDeviceLocation)
                    {
                        IsBusy = true;
                        try
                        {
                            var addressList = await Utils.GetDeviceAddressAsync();
                            if (addressList != null)
                            {
                                var address = addressList.FirstOrDefault().Split(',');
                                if (address.Length == 4)
                                {
                                    var stateZip = address[2].Trim().Split(' ');
                                    Customer.HomeAddress.Address1 = address[0].Trim();
                                    Customer.HomeAddress.Address2 = string.Empty;
                                    Customer.HomeAddress.City = address[1].Trim();
                                    Customer.HomeAddress.State = stateZip[0].Trim();
                                    Customer.HomeAddress.Zip = stateZip[1].Trim();
                                    Customer.HomeAddress.NotififyAll();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.Toast($"Unable to get device address: {ex.Message}");
                        }
                        finally { IsBusy = false; }
                    }
                    else
                    {
                        var customer = JsonConvert.DeserializeObject<CustomerModel>(OriginalCustomer);
                        Customer.HomeAddress = customer.Address;
                        Customer.HomeAddress.NotififyAll();
                    }
                });
            }
        }

        public ICommand GoToSchedulerCommand
        {
            get => new RelayCommand(() => GoToScheduler());
        }

        private async void GoToScheduler()
        {
            if (IsBusy || Scheduler.Any()) { return; }
            IsBusy = true;

            try
            {
                await NavigationService.NavigateToDialog(Locator.Scheduler);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await userDialogs.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region Methods 
        private void MakeBillingAddressSameHomeAddress()
        {
            if (Customer.SameHomeAddress
                && !string.IsNullOrEmpty(Customer?.HomeAddress?.FullAddress))
            {

                var cloned = (AddressModel)new AddressModel().InjectFrom(Customer.HomeAddress);
                Customer.Address = cloned;
                Customer.Address.NotififyAll();

                if (InitCompleted)
                {
                    IsEditing = true;
                }
            }
            else if(!Customer.SameHomeAddress)
            {
                var customer = JsonConvert.DeserializeObject<CustomerModel>(OriginalCustomer);
                if (customer != null)
                {
                    Customer.Address = customer.Address;
                    Customer.Address.NotififyAll();
                }
            }
        }

        private void SubscribeMessage()
        {
            Notify.SubscribeVisitingDayActionAction(async (sender) => {
                if (sender.Object is SchedulerModel scheduler)
                {
                    Customer.Scheduler?.Clear();

                    if (scheduler.Selected)
                    {
                        Customer.Scheduler.Add(scheduler);
                    }
                    
                    IsEditing = true;

                    OnPropertyChanged("Customer");
                }

                Customer.Scheduler = new List<SchedulerModel>(await GetScheduler(Customer));
            });
        }

        public bool InitCompleted { get; set; } = false;
        public void Init(CustomerModel customer = null)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                InitCompleted = false;
                IsEditing = false;
                ErrorMessage = "";
                OriginalCustomer = "";
                Scheduler = await GetScheduler(customer);

                if (customer != null)
                {
                    customer.Pool.Images = new System.Collections.ObjectModel.ObservableCollection<EntityImageModel>(await new ImageController().LocalData.List(new SQLControllerListCriteriaModel
                    {
                        Filter = new List<SQLControllerListFilterField> {
                              new SQLControllerListFilterField
                              {
                                  FieldName = "EntityId",
                                  ValueLBound = customer.Pool.Id.ToString()
                              }
                        }
                    }));

                    Customer = (CustomerModel)new CustomerModel().InjectFrom(customer);

                    if (Customer.Contact == null)
                    {
                        Customer.Contact = new ContactModel();
                    }

                    OriginalCustomer = JsonConvert.SerializeObject(customer,
                                Formatting.Indented,
                                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

                    MakeBillingAddressSameHomeAddress();
                }

                InitCompleted = true;
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

            if (customer != null && customer.Scheduler != null)
            {
                schedulers.Where(x => customer.Scheduler.Any(s => s.Id == x.Id)).ForEach((s) =>
                {
                    s.Selected = true;
                });
            }

            return schedulers;
        }
        #endregion
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