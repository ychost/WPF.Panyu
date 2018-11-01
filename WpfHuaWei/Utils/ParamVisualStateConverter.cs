using HuaWeiBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfHuaWei.Utils
{
    public class ParamVisualStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double result = System.Convert.ToSingle(value);
            string fieldName = System.Convert.ToString(parameter);
            Dictionary<string, DataPattern> dic = 
                App.Current.Properties["JhtStock"] as Dictionary<string, DataPattern>;
            DataPattern dp = dic[fieldName] as DataPattern;
            if(dp != null)
            {
                if(result > GlobalConstants.MinDisplayValue)
                {
                    if(result > dp.Upper || result < dp.Lower)
                        return Brushes.Red;
                    else
                        return Brushes.DarkGreen;
                }
            }
            return new SolidColorBrush(Color.FromRgb(0x04, 0x22, 0x71));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
