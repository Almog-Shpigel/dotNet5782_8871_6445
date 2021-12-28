using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PL
{
    public class BatteryToProgressBarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class ColorByPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((double)value > 75)
            {
                return Brushes.ForestGreen;
            }
            else if ((double)value > 50)
            {
                return Brushes.LimeGreen;
            }
            else if ((double)value > 25)
            {
                return Brushes.Orange;
            }
            else if ((double)value > 10)
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.DarkRed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
