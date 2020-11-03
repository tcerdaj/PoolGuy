using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.Services;
using GalaSoft.MvvmLight.Ioc;
using PoolGuy.Mobile.Services.Interface;
using System.Threading.Tasks;
using PoolGuy.Mobile.CustomControls;

namespace PoolGuy.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private ContentPage View;

        public void SetView(ContentPage view)
        {
            this.View = view;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public INavigationService NavigationService
        {
            get { return SimpleIoc.Default.GetInstance<INavigationService>(); }
        }

        
        public void ToggleFocus(string controlName, ControlTypeEnum type, bool focus)
        {
            if (View == null)
            {
                return;
            }

            var control = GetControl(controlName, type);

            if (control != null)
            {
                if (focus)
                {
                    if (control.IsFocused)
                    {
                        control.Unfocus();
                    }

                    control.Focus();
                }
                else
                {
                    control.Unfocus();
                }
            }
        }

        private View GetControl(string controlName, ControlTypeEnum controlType)
        {
            View control = null;

            if (View == null)
            {
                return control;
            }

            switch (controlType)
            {
                case ControlTypeEnum.ListView:
                    control = View.FindByName<ListView>(controlName);
                    break;
                case ControlTypeEnum.Picker:
                    control = View.FindByName<Picker>(controlName);
                    break;
                case ControlTypeEnum.EnhancedEntry:
                    control = View.FindByName<CustomEntry>(controlName);
                    break;
                case ControlTypeEnum.DatePicker:
                    control = View.FindByName<DatePicker>(controlName);
                    break;
                case ControlTypeEnum.TimePicker:
                    control = View.FindByName<TimePicker>(controlName);
                    break;
                case ControlTypeEnum.SearchBar:
                    control = View.FindByName<SearchBar>(controlName);
                    break;
                case ControlTypeEnum.StackLayout:
                    control = View.FindByName<StackLayout>(controlName);
                    break;
                case ControlTypeEnum.AbsoluteLayout:
                    control = View.FindByName<AbsoluteLayout>(controlName);
                    break;
                case ControlTypeEnum.Grid:
                    control = View.FindByName<Grid>(controlName);
                    break;
                default:
                    break;
            }

            return control;
        }

        #region UserDialogs
        public IUserDialogs Message
        {
            get
            {
                return SimpleIoc.Default.GetInstance<IUserDialogs>();
            }
        }
        #endregion UserDialogs
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            //let the UI know a property changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public enum ControlTypeEnum
    {
        Picker,
        EnhancedEntry,
        EnhancedStackLayout,
        AnimatedButton,
        DatePicker,
        SearchBar,
        AdjustableEditor,
        StackLayout,
        AbsoluteLayout,
        Grid,
        TimePicker,
        ListView
    }
}
