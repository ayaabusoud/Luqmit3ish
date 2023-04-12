using System;
using System.Globalization;
using Xamarin.Forms;

namespace Luqmit3ish.Models
{
    public class IndexToColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is int))
                return 0;

            int index = (int)value;

            return index % 3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

