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
using System.Xml.Serialization;
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
        [XmlIgnore]
        public CustomerModel Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private List<StopModel> _stopHistory;
        [XmlIgnore]
        public List<StopModel> StopHistory
        {
            get { return _stopHistory; }
            set { _stopHistory = value; OnPropertyChanged("StopHistory"); }
        }

        private StopModel _stop;
        [XmlIgnore]
        public StopModel Stop
        {
            get { return _stop; }
            set { _stop = value; OnPropertyChanged("Stop"); }
        }

        private void SubscribeMessage()
        {
         
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (Customer.StopId == Guid.Empty )
                {
                    var items = await new StopItemController().LocalData.List(new SQLControllerListCriteriaModel
                    {
                        Filter = new List<SQLControllerListFilterField> {
                         new SQLControllerListFilterField {
                             FieldName = "ItemType",
                             ValueLBound = ((int)eItemType.Stop).ToString()
                         }}
                    });

                    Stop = new StopModel
                    {
                        CustimerId = Customer.Id,
                        Customer = Customer,
                        Created = DateTime.Now,
                        Items = new ObservableCollection<StopItemModel>(items),
                        Status = Enums.WorkStatus.Working,
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
                    Stop = await new StopController().LoadAsync(Customer.StopId);
                }

                int weeks = 4;
                var weeksToRetrieve = DateTime.Now.AddDays(-(int)SelectedDate.DayOfWeek -(6 * weeks));
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

        public ICommand CompleteStopCommand
        {
            get
            {
                return new RelayCommand(async () => {
                    // TODO:
                    // Validate required fields

                    if (Customer.Status != Enums.WorkStatus.Completed)
                    {
                        Customer.Status = Enums.WorkStatus.Pending;
                    }

                    await NavigationService.CloseModal();
                });
            }
        }
    }
}