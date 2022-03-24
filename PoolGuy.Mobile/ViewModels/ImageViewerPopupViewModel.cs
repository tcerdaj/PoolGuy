using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace PoolGuy.Mobile.ViewModels
{
    public class ImageViewerPopupViewModel : BaseViewModel
    {
        public ImageViewerPopupViewModel()
        {
            
        }

        public ImageViewerPopupViewModel(string imageUrl)
        {
            _imageUrl = imageUrl;
            OnPropertyChanged("ImageUrl");
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
        }

        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    Helpers.Notify.RaiseImageViewerPopup(new Messages.RefreshMessage { Arg = "Closed" });
                    await NavigationService.PopPopupAsync();
                });
            }
        }
    }
}