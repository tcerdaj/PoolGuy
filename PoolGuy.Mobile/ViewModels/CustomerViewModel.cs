
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace PoolGuy.Mobile.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        public CustomerViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
        }

        public ICommand DisplayMessageCommand
        {
            get
            {
                return new RelayCommand(async() =>
                {
                    await Message.DisplayActionSheetCustomAsync("Action Sheet", "Cancel", "Option1", "Option2", "Option3");
                });
            }
        }
    }
}