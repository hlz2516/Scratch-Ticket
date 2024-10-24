using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ScratchTicket.Converters
{
    public class VariableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string res = "0";
            double num = Math.Round((double)value, 2, MidpointRounding.AwayFromZero);
            if (num > 0)
            {
                res = "+" + num.ToString();
            }
            else if(num < 0)
            {
                res = num.ToString();
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
