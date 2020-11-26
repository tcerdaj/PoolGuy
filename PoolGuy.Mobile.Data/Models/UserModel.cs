using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace PoolGuy.Mobile.Data.Models
{
    public class UserModel : EntityBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [ManyToMany(typeof(RoleModel))]
        public List<RoleModel> Roles { get; set; }
    }

    public class RoleModel: EntityBase
    {
        public string Name { get; set; }
        [ManyToMany(typeof(UserModel))]
        public List<UserModel> Users { get; set; }
    }

    public class UserRoleModel
    {
        [ForeignKey(typeof(UserModel))]
        public Guid UserId { get; set; }
        [ForeignKey(typeof(RoleModel))]
        public Guid RoleId { get; set; }
    }
}