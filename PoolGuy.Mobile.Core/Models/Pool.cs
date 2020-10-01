using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.Data.Models
{
    public class Pool: EntityBase
    {
        public PoolType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Surface { get; set; }
        public double Capacity { get; set; }
    }
}