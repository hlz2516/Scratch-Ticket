using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Data;

namespace ScratchTicket.Converters
{
    public class Bool2VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool hide)
            {
                if (hide)
                {
                    value = Visibility.Collapsed;
                }
                else
                {
                    value = Visibility.Visible;
                }
                return value;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Collapsed;
        }
    }
}
