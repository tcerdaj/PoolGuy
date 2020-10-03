using SQLite;
using System;

namespace PoolGuy.Mobile.Data.Models
{
    public abstract class EntityBase
    {
        [PrimaryKey]
        public virtual Guid Id { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Modified { get; set; }
    }
}