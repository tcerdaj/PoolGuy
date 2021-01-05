using System;

namespace PoolGuy.Mobile.Data.Models
{
    public class EntityImageModel : EntityBase
    {
        public Guid EntityId { get; set; }
        public Enums.ImageType ImageType { get; set; }
        public string ImageUrl { get; set; }
    }
}