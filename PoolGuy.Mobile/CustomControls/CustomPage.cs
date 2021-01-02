using Omu.ValueInjecter;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.CustomControls
{
    public class CustomPage : Page
    {
       public ePageType PageType { get; set; }

        private Page _page;
        public Page Page
        {
            get => _page;
        }
        

        public CustomPage()
        {

        }

        public CustomPage(Page v, ePageType type)
        {
            _page = v;
            this.PageType = type;
        }

        public CustomPage(PopupPage v)
        {
            _page = v;
            this.PageType = ePageType.Popup;
        }
    }
}
