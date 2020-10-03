using System;
using Xamarin.Forms;

namespace PoolGuy.Mobile.Converter
{
    public class StringEmptinessToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = value as string;
            bool reverse = parameter != null && parameter.ToString().ToLower() == "invert";
            bool val = string.IsNullOrWhiteSpace(str);

            return reverse ? val : !val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}