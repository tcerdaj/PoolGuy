using System;
using System.Collections;
using System.Linq;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Converter
{
    public class CollectionEmptyToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable ie = value as IEnumerable;
            bool reverse = parameter != null && parameter.ToString().ToLower() == "invert";
            bool val = ie == null || IsEmpty(ie);

            return reverse ? val : !val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private bool IsEmpty(IEnumerable en)
        {
            return !en.Cast<object>().Any();
        }
    }
}