using SQLite;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
    }
}