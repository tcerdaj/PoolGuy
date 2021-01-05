using PoolGuy.Mobile.Data.Models;

namespace PoolGuy.Mobile.ViewModels
{
    public class StopViewModel : BaseViewModel
    {
        public StopViewModel()
        {
            Title = this.GetType().Name.Replace("ViewModel", "");
            Globals.CurrentPage = Enums.ePage.Customer;
            SubscribeMessage();
            IsBusy = true;
        }

        private void SubscribeMessage()
        {
         
        }
    }
}