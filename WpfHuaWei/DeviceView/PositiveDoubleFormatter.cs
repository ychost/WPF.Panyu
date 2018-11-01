using HuaWeiBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WpfHuaWei.DeviceView
{
    /// <summary>
    /// 对正数进行格式化，小于等于0的数据返回一个特殊占位符
    /// </summary>
    public class PositiveDoubleFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            double doubleValue = (double)value;
            if (doubleValue > 0)
                return doubleValue.ToString(GlobalConstants.DoubleFormat);
            else
                return GlobalConstants.NonpositiveReplacer;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
