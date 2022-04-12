using PoolGuy.Mobile.CustomControls;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Services.Interface
{
    public interface INavigationService
    {
        Task NavigateToDialog(string pageKey);

        Task NavigateToDialog(string pageKey, object parameter, object parameter2 = null);

        Task NavigateToDialog(Page page);

        Task CloseAllAsync();

        bool? IsOnHomeView();

        Task CloseModal(bool animation = false);

        Task ReplaceRoot(Page page);

        Task ReplaceRoot(string page);

        Task PopToRootAsync();

        Task PushPopupAsync(string popUpPageKey, bool animate = false);

        Task PushPopupAsync(string popUpPageKey, object parameter, bool animate = false, CancellationToken cancelToken = default(CancellationToken));

        Task PopPopupAsync(bool animate = false);

        CustomPage CurrentPage { get; }
    }
}