using Plugin.Permissions.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolGuy.Mobile.Services.Interface
{
    public interface IPermissionService
    {
        Task<Dictionary<Permission, PermissionStatus>> CheckPermissions(params Permission[] permissions);
    }
}
