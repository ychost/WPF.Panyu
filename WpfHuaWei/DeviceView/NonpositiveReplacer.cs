using HuaWeiBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WpfHuaWei.DeviceView
{
    /// <summary>
    /// 使用一个特殊的占位符替换非正数
    /// </summary>
    public class NonpositiveReplacer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if ((double)value > 0)
                return ((double)value).ToString("#0");
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
