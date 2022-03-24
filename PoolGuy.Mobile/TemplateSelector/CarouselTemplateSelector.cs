using PoolGuy.Mobile.ViewModels;
using System;
using Xamarin.Forms;

namespace PoolGuy.Mobile.TemplateSelector
{
    public class CarouselTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                if (item is SearchCustomerPageViewModel page)
                {
                    return new DataTemplate(() => {
                        return ((ContentPage)page.Page).Content;
                    });
                }
                return DefaultTemplate;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
