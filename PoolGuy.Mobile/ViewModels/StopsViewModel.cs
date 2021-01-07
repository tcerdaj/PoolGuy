using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Data.Models.Query;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace PoolGuy.Mobile.ViewModels
{
    public class StopsViewModel : BaseViewModel
    {
        public StopsViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
            Globals.CurrentPage = Enums.ePage.Stops;
            SubscribeMessage();
            IsBusy = true;
        }

        private void SubscribeMessage()
        {
         
        }

        public string DayOfWeek
        {
            get 
            {
                return DateTime.Now.DayOfWeek.ToString();
            }
        }

        private List<CustomerModel> _stops;
        public List<CustomerModel> Stops
        {
            get { return _stops; }
            set { _stops = value; OnPropertyChanged("Stops"); }
        }

        public async Task RefreshStopsAsync()
        {
            try
            {
                var sch = await new SchedulerController().ListWithChildrenAsync(
                    new SQLControllerListCriteriaModel
                    {
                        Filter = new List<SQLControllerListFilterField> {
                        new SQLControllerListFilterField {
                            FieldName = "LongName",
                            ValueLBound = DayOfWeek
                        }}
                    });

                if (sch != null && sch.Any())
                {
                    Stops = sch.FirstOrDefault()
                        .Customers
                        .OrderBy(x=>x.Index)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(e.Message, Title, "Ok");
            }
        }

        public ICommand NavigateToCommand
        {
            get
            {
                return new RelayCommand<Enums.ePage>(async (item) =>
                {
                    string page = item.ToString();
                    await NavigationService.ReplaceRoot($"{page}Page");
                });
            }
        }
    }
}