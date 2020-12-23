﻿using GalaSoft.MvvmLight.Ioc;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using PoolGuy.Mobile.Services;
using PoolGuy.Mobile.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using PoolGuy.Mobile.Extensions;

[assembly: Dependency(typeof(PermissionService))]
namespace PoolGuy.Mobile.Services
{

    public class PermissionService : IPermissionService
    {
        SemaphoreSlim refreshWOSemaphore = new SemaphoreSlim(1);
        /// <summary>
        /// Check an array of permissions
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns>Permission, PermissionStatus</returns>
        public async Task<Dictionary<Permission, PermissionStatus>> CheckPermissions(params Permission[] permissions)
        {
            Dictionary<Permission, PermissionStatus> results = new Dictionary<Permission, PermissionStatus>();
            bool redirectToSettings = false;
            CancellationTokenSource token = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            await refreshWOSemaphore.WaitAsync();

            try
            {
                foreach (var permission in permissions)
                {
                    // CheckPermissionStatus
                    PermissionStatus status = await CheckPermissionStatusAsync(permission).WaitOrCancel(token.Token);

                    if (status == PermissionStatus.Granted)
                    {
                        results.Add(permission, status);
                        continue;
                    }

                    // Request permission
                    bool shouldShow = await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission)
                        .WaitOrCancel(token.Token);
                    var userDialog = SimpleIoc.Default.GetInstance<IUserDialogs>();

                    if (shouldShow && userDialog != null)
                    {
                        await userDialog.DisplayAlertAsync($"Need {permission.ToString()} permission", "Device permission", "Ok");
                    }

                    status = await RequestPermissionAsync(permission, token.Token).WaitOrCancel(token.Token);
                    if (status == PermissionStatus.Granted)
                    {
                        results.Add(permission, status);
                        continue;
                    }

                    results.Add(permission, status);

                    if (userDialog == null)
                    {
                        continue;
                    }

                    // The OS will only ever prompt the user once. If they deny permission,
                    // we have to display our custom permission request message.
                    string permissionName = Enum.GetName(typeof(Permission), permission) ?? "Unknown";

                    bool goToSettings =
                        await userDialog.DisplayConfirmationAsync(
                            $"To use this plug-in, {permissionName?.ToLower()} permission is required.",
                            $"{permissionName} Permission", "Settings", "Maybe Later");

                    if (goToSettings)
                    {
                        redirectToSettings = true;
                    }
                }

                if (redirectToSettings)
                {
                    CrossPermissions.Current.OpenAppSettings();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            finally 
            {
                refreshWOSemaphore.Release();
            }

            return results;
        }

        #region Helpers
        /// <summary>
        /// Check a permission status
        /// </summary>
        /// <param name="permission"></param>
        /// <returns>PermissionStatus</returns>
        private static async Task<PermissionStatus> CheckPermissionStatusAsync(Permission permission)
        {
            PermissionStatus status = PermissionStatus.Denied;

            try
            {
                switch (permission)
                {
                    case Permission.Unknown:
                        break;
                    case Permission.Calendar:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<CalendarPermission>();
                        break;
                    case Permission.Camera:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                        break;
                    case Permission.Contacts:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<ContactsPermission>();
                        break;
                    case Permission.Location:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
                        break;
                    case Permission.Microphone:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<MicrophonePermission>();
                        break;
                    case Permission.Phone:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<PhonePermission>();
                        break;
                    case Permission.Photos:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>();
                        break;
                    case Permission.Reminders:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<RemindersPermission>();
                        break;
                    case Permission.Sensors:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<SensorsPermission>();
                        break;
                    case Permission.Sms:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<SmsPermission>();
                        break;
                    case Permission.Storage:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                        break;
                    case Permission.Speech:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<SpeechPermission>();
                        break;
                    case Permission.LocationAlways:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationAlwaysPermission>();
                        break;
                    case Permission.LocationWhenInUse:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationWhenInUsePermission>();
                        break;
                    case Permission.MediaLibrary:
                        status = await CrossPermissions.Current.CheckPermissionStatusAsync<MediaLibraryPermission>();
                        break;
                    default:
                        throw new NotImplementedException($"{permission.ToString()} has not been implemented.");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }

            return status;
        }

        /// <summary>
        /// Request a permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns>PermissionStatus</returns>
        private static async Task<PermissionStatus> RequestPermissionAsync(Permission permission, CancellationToken token = new CancellationToken())
        {
            PermissionStatus status = PermissionStatus.Denied;

            try
            {
                if (token.IsCancellationRequested)
                {
                    return status;
                }

                switch (permission)
                {
                    case Permission.Unknown:
                        break;
                    case Permission.Calendar:
                        status = await CrossPermissions.Current.RequestPermissionAsync<CalendarPermission>();
                        break;
                    case Permission.Camera:
                        status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                        break;
                    case Permission.Contacts:
                        status = await CrossPermissions.Current.RequestPermissionAsync<ContactsPermission>();
                        break;
                    case Permission.Location:
                        status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
                        break;
                    case Permission.Microphone:
                        status = await CrossPermissions.Current.RequestPermissionAsync<MicrophonePermission>();
                        break;
                    case Permission.Phone:
                        status = await CrossPermissions.Current.RequestPermissionAsync<PhonePermission>();
                        break;
                    case Permission.Photos:
                        status = await CrossPermissions.Current.RequestPermissionAsync<PhotosPermission>();
                        break;
                    case Permission.Reminders:
                        status = await CrossPermissions.Current.RequestPermissionAsync<RemindersPermission>();
                        break;
                    case Permission.Sensors:
                        status = await CrossPermissions.Current.RequestPermissionAsync<SensorsPermission>();
                        break;
                    case Permission.Sms:
                        status = await CrossPermissions.Current.RequestPermissionAsync<SmsPermission>();
                        break;
                    case Permission.Storage:
                        status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                        break;
                    case Permission.Speech:
                        status = await CrossPermissions.Current.RequestPermissionAsync<SpeechPermission>();
                        break;
                    case Permission.LocationAlways:
                        status = await CrossPermissions.Current.RequestPermissionAsync<LocationAlwaysPermission>();
                        break;
                    case Permission.LocationWhenInUse:
                        status = await CrossPermissions.Current.RequestPermissionAsync<LocationWhenInUsePermission>();
                        break;
                    case Permission.MediaLibrary:
                        status = await CrossPermissions.Current.RequestPermissionAsync<MediaLibraryPermission>();
                        break;
                    default:
                        throw new NotImplementedException($"{permission.ToString()} has not been implemented.");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return status;
            }

            return status;
        }
        #endregion
    }
}