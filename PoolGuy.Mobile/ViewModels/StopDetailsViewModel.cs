using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Extensions;

namespace PoolGuy.Mobile.ViewModels
{
    public class StopDetailsViewModel : BaseViewModel
    {
        public StopDetailsViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "").SplitWord();
            Globals.CurrentPage = Enums.ePage.Customer;
            SubscribeMessage();
            IsBusy = true;
        }

        private void SubscribeMessage()
        {
         
        }
    }
}