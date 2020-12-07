using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using PoolGuy.Mobile.Models;
using System.Linq;
using System.Diagnostics;

namespace PoolGuy.Mobile.ViewModels
{
    public class ActionSheetPopupViewModel : BaseViewModel
    {
        public ActionSheetPopupViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
        }


        public eContentType ContentType { get; set; }
        public ActionSheetPopupViewModel(ActionSheetModel model)
        {
            Title = model.Title;
            ButtonLabels = model.Buttons.ToList();
            CancelLabel = model.Cancel;
            ContentType = model.ContentType;
        }

        private List<string> buttonLabels;
        public List<string> ButtonLabels
        {
            get { return buttonLabels; }
            set { buttonLabels = value; OnPropertyChanged("ButtonLabels"); }
        }

        private string cancelLabel;
        public string CancelLabel
        {
            get { return cancelLabel; }
            set { cancelLabel = value; OnPropertyChanged("CancelLabel"); }
        }

        public ICommand SelectActionCommand
        {
            get
            {
                return new RelayCommand<string>(async (action) =>
                {
                    await SelectActionAsync(action);
                });
            }
        }

        bool _blockSelectAction;
        private async Task SelectActionAsync(string action = null)
        {
            if (_blockSelectAction) return;

            _blockSelectAction = true;

            try
            {
                System.Diagnostics.Debug.WriteLine($"Your selection {action}");

                await NavigationService.PopPopupAsync(false);

                Helpers.Notify.RaiseActionSheetPopup(new Messages.RefreshMessage { Arg = action });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                _blockSelectAction = false;
            }
        }
    }
}
