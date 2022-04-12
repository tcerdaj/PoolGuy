using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.Ioc;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.CustomControls;
using System.Xml.Serialization;

namespace PoolGuy.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler NotifyView;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            //let the UI know a property changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnNotifyView(string propertyName)
        {
            //let the UI know a property changed.
            NotifyView?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged("Title"); }
        }

        private string _status = "";
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        [XmlIgnore]
        public CustomPage CurrentPage
        {
            get => NavigationService.CurrentPage;
        }

        public INavigationService NavigationService
        {
            get { return SimpleIoc.Default.GetInstance<INavigationService>(); }
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
        
    }
}