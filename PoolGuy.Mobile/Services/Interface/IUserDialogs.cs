using Acr.UserDialogs;
using System;
using System.Threading.Tasks;

namespace PoolGuy.Mobile.Services.Interface
{
    public interface IUserDialogs
    {
        Task DisplayAlertAsync(string message, string title, string cancel = "Ok");
        Task<bool> DisplayConfirmationAsync(string message, string title, string accept = "Ok", string cancel = "Cancel");
        Task<string> DisplayActionSheetAsync(string title, string cancel, params string[] buttons);
        Task<string> DisplayActionSheetCustomAsync(string title, string cancel, params string[] buttons);
        Task<PromptResult> DisplayPromptAsync(string message, string title, string accept = "Ok", string cancel = "Cancel", string placeholder = null, Acr.UserDialogs.InputType keyboard = InputType.Default);
        void Toast(string message, TimeSpan? duration = null);
        void Toast(ToastConfig toastConfig);
    }
}