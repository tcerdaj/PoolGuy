using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace PoolGuy.Mobile.Services.Interface
{
    public interface IImageService
    {
        Task<MediaFile> TakePhoto(string action);
    }
}