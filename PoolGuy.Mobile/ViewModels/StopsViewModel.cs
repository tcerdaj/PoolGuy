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

        private DateTime currentDate = DateTime.Now;

        public string DayOfWeek
        {
            get 
            {
                return currentDate.DayOfWeek.ToString();
            }
        }

        private List<CustomerModel> _stops;
        public List<CustomerModel> Stops
        {
            get { return _stops; }
            set { _stops = value; OnPropertyChanged("Stops"); }
        }

        private SchedulerModel _sch;

        public async Task RefreshStopsAsync(eDirection direction = eDirection.None)
        {
            try
            {
                if (string.IsNullOrEmpty(DayOfWeek))
                {
                    return;
                }

                var schs = await new SchedulerController().ListWithChildrenAsync();
                var sch = schs.Where(x => x.LongName == DayOfWeek);

                if (sch != null && sch.Any())
                {
                    _sch = sch.FirstOrDefault();
                    Stops = sch.FirstOrDefault()
                        .Customers
                        .OrderBy(x => x.Index)
                        .ToList();
                }
                else if(_sch != null)
                {
                    int goTo = direction == eDirection.Next? 1: direction == eDirection.Previus? -1: 0;
                    var ind = schs.IndexOf(_sch) + goTo;
                    if (ind > 0)
                    {
                        _sch = schs[schs.IndexOf(_sch) + goTo];

                        Stops = _sch
                            .Customers
                            .OrderBy(x => x.Index)
                            .ToList();
                    }
                    else
                    {
                        Stops = new List<CustomerModel>();
                    }
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

        public ICommand GoToWeekDayCommand
        {
            get
            {
                return new RelayCommand<string>(async (direction) =>
                {
                    if (direction == "Previus")
                    {
                        currentDate = currentDate.AddDays(-1);
                        await RefreshStopsAsync(eDirection.Previus);
                        OnPropertyChanged("DayOfWeek");
                    }
                    else if (direction == "Next")
                    {
                        currentDate = currentDate.AddDays(1);
                        await RefreshStopsAsync(eDirection.Next);
                        OnPropertyChanged("DayOfWeek");
                    }
                });
            }
        }

        public enum eDirection
        {
            None,
            Next,
            Previus
        }
    }
}