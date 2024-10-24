using ScratchTicket.ORM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ScratchTicket.Converters
{
    public class String2BundleTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CardBundleType res = CardBundleType.Normal;
            string typeStr = value as string;
            if (!string.IsNullOrEmpty(typeStr))
            {
                switch (typeStr)
                {
                    case "Normal":
                    case "normal":
                        res = CardBundleType.Normal;
                        break;
                    case "Rare":
                    case "rare":
                        res = CardBundleType.Rare;
                        break;
                    case "Legend":
                    case "legend":
                        res = CardBundleType.Legend;
                        break;
                    default:
                        break;
                }
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
