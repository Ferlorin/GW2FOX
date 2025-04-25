using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace GW2FOX
    {
        public class StrikeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (value is bool b && b)
                    ? TextDecorations.Strikethrough
                    : new TextDecorationCollection(); // Leere Liste statt null
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    }


    public class ItalicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? FontStyles.Italic : FontStyles.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
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
                _ => System.Windows.Media.Brushes.White
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



}
