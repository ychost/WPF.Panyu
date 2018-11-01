using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WpfHuaWei.DeviceView
{
    public class ColorNameToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            string colorName = (string)value;
            if (string.IsNullOrEmpty(colorName))
                return System.Windows.Media.Brushes.Black;
            if (colorName.Contains("黑"))
                return System.Windows.Media.Brushes.Black;
            if (colorName.Contains("兰") || colorName.Contains("蓝"))
                return System.Windows.Media.Brushes.CornflowerBlue;
            if (colorName.Contains("红"))
                return System.Windows.Media.Brushes.Red;
            if (colorName.Contains("黄"))
                return System.Windows.Media.Brushes.Yellow;
            if (colorName.Contains("白"))
                return System.Windows.Media.Brushes.White;
            if (colorName.Contains("紫"))
                return System.Windows.Media.Brushes.Purple;
            if (colorName.Contains("橙"))
                return System.Windows.Media.Brushes.Orange;
            if (colorName.Contains("绿"))
                return System.Windows.Media.Brushes.Green;
            if (colorName.Contains("棕"))
                return System.Windows.Media.Brushes.Brown;
            if (colorName.Contains("灰"))
                return System.Windows.Media.Brushes.Gray;
            return System.Windows.Media.Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
