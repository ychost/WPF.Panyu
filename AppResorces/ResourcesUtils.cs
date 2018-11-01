using AppResources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AppResorces
{
    /// <summary>
    /// 用于管理应用程序使用到的内部或者外部资源
    /// </summary>
    public class ResourcesUtils
    {
        /// <summary>
        /// 获取dll内嵌的图片资源
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns>将资源作为ImageSource返回</returns>
        public static ImageSource GetImageSource(string imageName)
        {
            ResourceHelper helper = new ResourceHelper();
            try
            {
                Stream stream = helper.assembly.GetManifestResourceStream(
                    "AppResources.Resources.Images." + imageName);
                Bitmap bitmap = new Bitmap(stream);
                return helper.ChangeBitmapToImageSource(bitmap);
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 获取映射的字符串资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetMappedString(string key)
        {
            if(StringDictionary.StringRes.ContainsKey(key))
                return StringDictionary.StringRes[key];
            return string.Empty;
        }
    }
}
