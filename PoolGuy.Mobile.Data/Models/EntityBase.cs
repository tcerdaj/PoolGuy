using SQLite;
using System;
using System.ComponentModel;

namespace PoolGuy.Mobile.Data.Models
{
    public abstract class EntityBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        [PrimaryKey]
        public virtual Guid Id { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual bool WasModified { get; set;}
    }
}