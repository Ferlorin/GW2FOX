using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GW2FOX
{
    public class PastEventColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool isPast && isPast ? System.Windows.Media.Brushes.Gray : System.Windows.Media.Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
