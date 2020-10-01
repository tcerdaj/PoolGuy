using SQLite;

namespace PoolGuy.Mobile.Data.Models
{
    public class Customer : EntityBase
    {
        [Unique]
        [NotNull]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public byte[] Photo { get; set; }
    }
}