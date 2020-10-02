using System;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using PoolGuy.Mobile.Helpers;
using GalaSoft.MvvmLight.Ioc;
using PoolGuy.Mobile.Services.Interface;
using PoolGuy.Mobile.Models;

[assembly:Dependency(typeof(PoolGuy.Mobile.Services.UserDialogs))]
namespace PoolGuy.Mobile.Services
{
    public class UserDialogs : PoolGuy.Mobile.Services.Interface.IUserDialogs
    {
        private CancellationTokenSource _cancellationToken;
        private static readonly EventWaitHandle WaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static string _action = string.Empty;

        public async Task<string> DisplayActionSheetAsync(string title, string cancel, params string[] buttons)
        {
            try
            {
                _cancellationToken?.Cancel();
                _cancellationToken = new CancellationTokenSource();
                if (Device.RuntimePlatform == Device.iOS)
                {
                    await Task.Delay(500);
                }

                return await Device.InvokeOnMainThreadAsync(async () =>
                    await Acr.UserDialogs.UserDialogs.Instance.ActionSheetAsync(title, cancel, null, _cancellationToken.Token, buttons));
            }
            catch (TaskCanceledException)
            {
                return cancel;
            }
        }

        public async Task<string> DisplayActionSheetCustomAsync(string title, string cancel, params string[] buttons)
        {
            _action = string.Empty;
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();
            if (Device.RuntimePlatform == Device.iOS)
            {
                await Task.Delay(500);
            }

            try
            {
                Notify.SubscribeActionSheetPopup((sender) =>
                {
                    _action = sender.Arg;
                    WaitHandle.Set();
                });

                await SimpleIoc.Default.GetInstance<INavigationService>().PushPopupAsync(Locator.Popup.ActionSheetPopup,
                    new ActionSheetModel { Title = title, Cancel = cancel, Buttons = buttons }, false, _cancellationToken.Token);

                await Task.Run(() => WaitHandle.WaitOne());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return _action;
        }

        public Task DisplayAlertAsync(string message, string title, string cancel = "Ok")
        {
            throw new NotImplementedException();
        }

        public Task<bool> DisplayConfirmationAsync(string message, string title, string accept = "Ok", string cancel = "Cancel")
        {
            throw new NotImplementedException();
        }

        public Task<PromptResult> DisplayPromptAsync(string message, string title, string accept = "Ok", string cancel = "Cancel", string placeholder = null, InputType keyboard = InputType.Default)
        {
            throw new NotImplementedException();
        }

        public void Toast(string message, TimeSpan? duration = null)
        {
            throw new NotImplementedException();
        }

        public void Toast(ToastConfig toastConfig)
        {
            throw new NotImplementedException();
        }
    }
}