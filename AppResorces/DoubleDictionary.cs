using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppResources
{
    internal class DoubleDictionary
    {
        /// <summary>
        /// 用一个字典保存应用程序用到的资源
        /// 它的作用于Application.Current.Properties是一样的，
        /// 只不过专门独立出主应用程序之外，放到dll中来
        /// </summary>
        public readonly static Dictionary<string, double> DoubleRes = new Dictionary<string, double>();

        static DoubleDictionary()
        {
            
        }
    }
}
