﻿using SQLite;
using System;
using System.ComponentModel;

namespace PoolGuy.Mobile.Data.Models
{
    public abstract class EntityBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [PrimaryKey]
        public virtual Guid Id { get; set; }
        public virtual DateTime? Created { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual bool WasModified { get; set; }
       
        private bool _selected;
        [Ignore]
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("Selected"); }
        }

        public void NotififySelected()
        {
            OnPropertyChanged("Selected");
        }

        public void NotififyImageUrl()
        {
            OnPropertyChanged("ImageUrl");
        }

        public void NotififyShortName()
        {
            OnPropertyChanged("ShortName");
        }
    }
}