using System;
using System.Collections;
using System.Linq;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Converter
{
    public class CollectionListToCount : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable ie = value as IEnumerable;
            int val = ie == null? 0 : Count(ie);

            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private int Count(IEnumerable en)
        {
            return en.Cast<object>().Count();
        }
    }
}