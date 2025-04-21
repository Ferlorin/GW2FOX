using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.TextFormatting;

namespace GW2FOX
{
    public class StrikeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TextDecorations.Strikethrough : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
