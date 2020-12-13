using PoolGuy.Mobile.ViewModels;
using System;
using Xamarin.Forms;

namespace PoolGuy.Mobile.TemplateSelector
{
    public class EquipmentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate ManufactureTemplate { get; set; }
        public DataTemplate EquipmentTemplate { get; set; }
        public DataTemplate EquipmentModelTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                DataTemplate result = DefaultTemplate;
                switch (Globals.CurrentPage)
                {
                    case Data.Models.Enums.ePage.EquipmentSelectModel:
                        result = EquipmentModelTemplate;
                        break;
                    case Data.Models.Enums.ePage.SelectManufacture:
                        result = ManufactureTemplate;
                        break;
                    case Data.Models.Enums.ePage.Equipment:
                        result = EquipmentTemplate;
                        break;
                    default:
                        break;
                }

                return result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }
        }
    }
}
