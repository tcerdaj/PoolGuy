using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.Query;
using PoolGuy.Mobile.Extensions;
using PoolGuy.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.ViewModels
{
    public class StopDetailsViewModel : BaseViewModel
    {
        public StopDetailsViewModel(MobileCustomerModel customer)
        {
            Title = this.GetType().Name.Replace("ViewModel", "").SplitWord();
            Globals.CurrentPage = Enums.ePage.StopDetails;
            
            if (customer.Customer.Status == Enums.WorkStatus.None || customer.Customer.Status == Enums.WorkStatus.Pending)
            {
                customer.Customer.Status = Enums.WorkStatus.Working;
            }

            Customer = customer.Customer;
            SelectedDate = customer.SelectedDate;
            SubscribeMessage();
            IsBusy = true;
        }

        public DateTime? DateLastVisit
        {
            get 
            {
                return Customer.DateLastVisit.Value.ToLocalTime();
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate 
        {
            get { return _selectedDate; }
            set { _selectedDate = value; OnPropertyChanged("SelectedDate"); } 
        }

        public string Year
        {
            get 
            {
                string year = DateTime.Now.ToString("yyyy");
                if (StopHistory != null && StopHistory.Any(x => x.Created.Value.Year != DateTime.Now.Year))
                {
                    var y = StopHistory.FirstOrDefault(x => x.Created.Value.Year != DateTime.Now.Year).Created.Value.ToString("yy");
                    year = $"{year.Substring(2, 2)}-{y}";
                }

                return year;
            }
        }

        private CustomerModel _customer;
        public CustomerModel Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private List<StopModel> _stopHistory;
        public List<StopModel> StopHistory
        {
            get { return _stopHistory; }
            set { _stopHistory = value; OnPropertyChanged("StopHistory"); }
        }

        private StopModel _stop;
        public StopModel Stop
        {
            get { return _stop; }
            set { _stop = value; OnPropertyChanged("Stop"); }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get { return _isEditing; }
            set {
                if (Stop?.Status != WorkStatus.Completed)
                {
                    _isEditing = value;
                    OnPropertyChanged(nameof(IsEditing));
                }
            }
        }

        private void SubscribeMessage()
        {
         
        }


        public bool InitCompleted { get; set; }

        public async Task InitializeAsync()
        {
            InitCompleted = false;

            try
            {
                var stops = await new StopController().ListWithChildrenAsync(new SQLControllerListCriteriaModel
                {
                    Filter = new List<SQLControllerListFilterField>
                    {
                        new SQLControllerListFilterField() {
                            FieldName = "CustomerId",
                            ValueLBound = Customer.Id.ToString()
                        }
                    }
                });

                stops = stops.Where(x => x.SelectedDate.Date == SelectedDate.Date).ToList();

                if (!stops.Any())
                {
                    var items = await new StopItemController().LocalData.List(new SQLControllerListCriteriaModel
                    {
                        Filter = new List<SQLControllerListFilterField> {
                             new SQLControllerListFilterField {
                                 FieldName = "ItemType",
                                 ValueLBound = ((int)eItemType.Stop).ToString()
                             }}
                    });

                    items = items.GroupBy(g => g.Name).Select(x => x.FirstOrDefault()).OrderBy(o => o.Index).ToList();

                    // reset item values
                    foreach (var item in items)
                    {
                        item.Id = Guid.NewGuid();
                        item.Test = null;
                        item.Value = null;
                        item.Appliyed = null;
                        item.Suggested = null;
                    }

                    var itemsResult = await AddItemsPrompt(items);

                    Stop = new StopModel
                    {
                        CustomerId = Customer.Id,
                        Customer = Customer,
                        Created = DateTime.Now,
                        Items = itemsResult,
                        Status = Enums.WorkStatus.Working,
                        SelectedDate = SelectedDate.Date,
                        User = new UserModel
                        {
                            Id = Guid.Parse("a3dab4d2-81d6-40a9-a63e-a013f825ba71"),
                            FirstName = "Teo",
                            LastName = "Cerda",
                            Password = "123",
                            UserName = "tcerdaj@hotmail.com",
                            Roles = new List<RoleModel> {
                                new RoleModel
                                {
                                    Id = Guid.Parse("247e4561-1c3d-49da-aa2d-dd55902ade73"),
                                    Name = "Admin"
                                }
                            }
                        }
                    };
                }
                else
                {
                    // TODO: apply logic to create or select the last stop
                    Stop = stops.LastOrDefault();
                    Stop.Items = await AddItemsPrompt(Stop.Items.ToList());
                }

                int weeks = 4;
                var weeksToRetrieve = DateTime.Now.AddDays(-(int)SelectedDate.DayOfWeek - (6 * weeks));
                List<StopModel> stopHistory = new List<StopModel>();
                stopHistory = await new StopController().LocalData.List(new SQLControllerListCriteriaModel
                {
                    Filter = new List<SQLControllerListFilterField>
                    {
                        new SQLControllerListFilterField
                        {
                             FieldName = "Created",
                             ValueLBound = weeksToRetrieve.ToString(),
                             DateKind = Data.Models.Query.SQLControllerListFilterField.DateKindEnum.Localized
                        }
                    }
                });

                if (!stopHistory.Any())
                {
                    stopHistory.Add(Stop);
                }

                StopHistory = stopHistory;
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally { InitCompleted = true; }
        }

        private async Task<ObservableCollection<StopItemModel>> AddItemsPrompt(List<StopItemModel> items)
        {
            if (!items.Any())
            {
                var result = await Message.DisplayConfirmationAsync("There is not items yet added to the stop, do you want to added now", Title, "Yes", "Cancel");
                if (result)
                {
                    await Utils.AddStopDetaultItemsAsync();

                    items = await new StopItemController().LocalData.List(new SQLControllerListCriteriaModel
                    {
                        Filter = new List<SQLControllerListFilterField> {
                                     new SQLControllerListFilterField {
                                         FieldName = "ItemType",
                                         ValueLBound = ((int)eItemType.Stop).ToString()
                                     }}
                    });
                }
            }

            return new ObservableCollection<StopItemModel>(items);
        }

        public ICommand GoBackCommand
        {
            get
            {
                return new RelayCommand(async () => {
                    
                    if (Customer.Status != Enums.WorkStatus.Completed)
                    {
                        Customer.Status = Enums.WorkStatus.Pending;
                    }

                    await NavigationService.CloseModal();
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand<bool>(async (complete) =>
                {
                    if (!IsEditing && complete)
                    {
                        return;
                    }

                    if (Stop.Items == null || (Stop.Items != null && !Stop.Items.Any()))
                    {
                        Message.Toast("Items are required", TimeSpan.FromSeconds(5));
                        return;
                    }

                    if (Stop.Items != null && Stop.Items.All(x=>!x.IsRequired && string.IsNullOrEmpty(x.Test) && string.IsNullOrEmpty(x.Appliyed)))
                    {
                        Message.Toast("You need to add a least an item value", TimeSpan.FromSeconds(5));
                        return;
                    }

                    var errors = ItemsValidation();
                    if (!string.IsNullOrEmpty(errors))
                    {
                        Message.Toast(errors, TimeSpan.FromSeconds(5));
                        return;
                    }

                    if (Stop.Status != WorkStatus.Completed && complete && await Message.DisplayConfirmationAsync("Are you sure do you want to complete the stop?", "Confirmation", "Ok", "Cancel"))
                    {
                        Stop.Status = WorkStatus.Completed;
                    }
                    else if(complete)
                    {
                        return;
                    }

                    await new StopController().ModifyWithChildrenAsync(Stop);

                    await NavigationService.CloseModal();
                });
            }
        }

        /// <summary>
        /// Validate required items
        /// </summary>
        /// <returns></returns>
        private string ItemsValidation()
        {
            // TODO:
            // Validate required fields
            List<string> errors = new List<string>();
            foreach (var item in Stop.Items)
            {
                if (item.IsRequired && (string.IsNullOrEmpty(item.Test) || string.IsNullOrEmpty(item.Appliyed)))
                {
                    errors.Add($"{item.Name} is required");
                }
            }

            if (!errors.Any())
            {
            return null;    
            }

            return string.Join(", ", errors);
            
        }
    }
}