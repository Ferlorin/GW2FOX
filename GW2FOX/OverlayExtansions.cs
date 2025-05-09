using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace GW2FOX
{
    public class StrikeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b)
                ? TextDecorations.Strikethrough
                : new TextDecorationCollection();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ItalicMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] is not BossListItem currentItem || values[1] is not IEnumerable items)
                return FontStyles.Normal;

            foreach (var other in items)
            {
                if (other is BossListItem otherItem &&
                    otherItem != currentItem &&
                    otherItem.NextRunTime == currentItem.NextRunTime)
                {
                    return FontStyles.Italic;
                }
            }

            return FontStyles.Normal;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }




    public class CategoryColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                "Maguuma" => System.Windows.Media.Brushes.LimeGreen,
                "Desert" => System.Windows.Media.Brushes.DeepPink,
                "WBs" => System.Windows.Media.Brushes.WhiteSmoke,
                "Ice" => System.Windows.Media.Brushes.DeepSkyBlue,
                "Cantha" => System.Windows.Media.Brushes.Blue,
                "SotO" => System.Windows.Media.Brushes.Yellow,
                "LWS2" => System.Windows.Media.Brushes.LightYellow,
                "LWS3" => System.Windows.Media.Brushes.ForestGreen,
                "Treasures" => System.Windows.Media.Brushes.DarkRed,
                _ => System.Windows.Media.Brushes.White
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
