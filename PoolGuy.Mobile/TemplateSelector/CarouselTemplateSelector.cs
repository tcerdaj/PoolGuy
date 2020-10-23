using System;
using System.Collections.Generic;
using System.Text;
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
                if (item is ContentPage page)
                {
                    var dt = new DataTemplate(() => {
                        return page.Content ;
                    });

                    return dt;
                    
                    switch (page.Title)
                    {
                        case "Login":
                            
                            break;
                        default:
                            break;
                    }
                }
                return DefaultTemplate;
            }
            catch (Exception)
            {

                throw;
            }

            throw new NotImplementedException();
        }
    }
}
