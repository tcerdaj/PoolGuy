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

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                DataTemplate result = DefaultTemplate;
                switch (Globals.CurrentPage)
                {
                    case Data.Models.Enums.ePage.Home:
                        break;
                    case Data.Models.Enums.ePage.SearchCustomer:
                        break;
                    case Data.Models.Enums.ePage.Customer:
                        break;
                    case Data.Models.Enums.ePage.SelectEquipment:
                        break;
                    case Data.Models.Enums.ePage.SelecteManufacture:
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
