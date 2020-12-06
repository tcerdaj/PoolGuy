using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using PoolGuy.Mobile.Services.Interface;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using PoolGuy.Mobile.Services;

[assembly: Dependency(typeof(ImageService))]
namespace PoolGuy.Mobile.Services
{
    public class ImageService : IImageService
    {
        public async Task<MediaFile> TakePhoto(string action)
        {
            MediaFile photo = null;

            if (action == null)
            {
                return null;
            }

            try
            {
                var status = await DependencyService.Get<IPermissionService>()
                    .CheckPermissions(Permission.Photos, Permission.Camera, Permission.Storage);

                if (!status.All(x => x.Value == PermissionStatus.Granted))
                {
                    return null;
                }


                if (action == "Gallery")
                {
                    photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                    {
                        PhotoSize = PhotoSize.Medium,
                        SaveMetaData = false,
                        MaxWidthHeight = 400
                    });
                }
                else 
                {
                    photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                    {
                        SaveToAlbum = true,
                        PhotoSize = PhotoSize.Medium,
                        SaveMetaData = false,
                        MaxWidthHeight = 400
                    });
                }

                return photo;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }

            return photo;
        }
    }
}