using PoolGuy.Mobile.Models;
using PoolGuy.Mobile.ViewModels;
using System;
using Xamarin.Forms;

namespace PoolGuy.Mobile.TemplateSelector
{
    public class ActionSheetTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ActionSheetTemplate { get; set; }
        public DataTemplate ImageViewerTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                if (container is StackLayout stack)
                {

                    if (stack.BindingContext is ActionSheetPopupViewModel vm)
                    {
                        if (vm. ContentType == eContentType.ImageUrl)
                        {
                            return ImageViewerTemplate;
                        }
                    }
                }

                return ActionSheetTemplate;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
