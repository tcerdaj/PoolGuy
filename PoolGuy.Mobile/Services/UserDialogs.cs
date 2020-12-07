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

        public async Task<string> DisplayActionSheetCustomAsync(string title, string cancel, eContentType contentType, params string[] buttons)
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
                    new ActionSheetModel { Title = title, Cancel = cancel, ContentType = contentType, Buttons = buttons }, false, _cancellationToken.Token);

                await Task.Run(() => WaitHandle.WaitOne());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return _action;
        }

        public async Task DisplayAlertAsync(string message, string title, string cancel = "Ok")
        {
            try
            {
                _cancellationToken?.Cancel();
                _cancellationToken = new CancellationTokenSource();
                if (Device.RuntimePlatform == Device.iOS)
                {
                    await Task.Delay(500);
                }

                await Device.InvokeOnMainThreadAsync(async () => await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(message, title, cancel, _cancellationToken.Token));
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine(e);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task<bool> DisplayConfirmationAsync(string message, string title, string accept = "Ok", string cancel = "Cancel")
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
                     await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(message, title, accept, cancel,
                         _cancellationToken.Token));
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine(e);
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<PromptResult> DisplayPromptAsync(string message, string title, string accept = "Ok", string cancel = "Cancel", string placeholder = null, InputType keyboard = InputType.Default)
        {
            try
            {
                _cancellationToken?.Cancel();
                _cancellationToken = new CancellationTokenSource();
                if (Device.RuntimePlatform == Device.iOS)
                {
                    await Task.Delay(500);
                }

                PromptResult result = await Device.InvokeOnMainThreadAsync(async () => await Acr.UserDialogs.UserDialogs.Instance.PromptAsync(message, title, accept, cancel, placeholder, keyboard, _cancellationToken.Token));
                return result;
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine(e);
                return new PromptResult(false, "");

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new PromptResult(false, "");
            }
        }

        public void Toast(string message, TimeSpan? duration = null)
        {
            Device.BeginInvokeOnMainThread(() => Acr.UserDialogs.UserDialogs.Instance.Toast(new ToastConfig(message)
            {
                Duration = duration ?? TimeSpan.FromSeconds(5),
                Position = ToastPosition.Bottom
            }));
        }

        public void Toast(ToastConfig toastConfig)
        {
            Device.BeginInvokeOnMainThread(() => Acr.UserDialogs.UserDialogs.Instance.Toast(toastConfig));
        }
    }
}