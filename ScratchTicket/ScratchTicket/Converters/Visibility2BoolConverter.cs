using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ScratchTicket.Converters
{
    public class Visibility2BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool b = (bool)value;
                if (b) //true是要隐藏，因为属性叫HidePrice
                {
                    value = Visibility.Collapsed;
                }
                else
                {
                    value = Visibility.Visible;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                var v = (Visibility)value;
                if (v == Visibility.Visible)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return value;
        }
    }
}
