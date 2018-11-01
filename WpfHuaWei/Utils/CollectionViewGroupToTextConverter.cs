using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfHuaWei.Utils
{
    public class CollectionViewGroupToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CollectionViewGroup group = value as CollectionViewGroup;
            if(group != null)
            {
                if(group.IsBottomLevel)
                    return group.Name + " ---- " + group.ItemCount + " 个模块";
                else
                    return group.Name + "工序";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
