using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfHuaWei.Utils
{
    public class CommonUtil
    {
        /// <summary>
        /// 根据机台名称判断该机台是不是挤绝缘机台
        /// </summary>
        /// <param name="machineNo"></param>
        /// <returns></returns>
        public static bool IsJjyMachine(string machineNo)
        {
            return !string.IsNullOrEmpty(machineNo) && machineNo.Contains("60");
        }

        /// <summary>
        /// 根据机台名称判断该机台是不是成缆机台
        /// </summary>
        /// <param name="machineNo"></param>
        /// <returns></returns>
        public static bool IsClMachine(string machineNo)
        {
            return !string.IsNullOrEmpty(machineNo) && machineNo.Contains("成缆");
        }

        /// <summary>
        /// 根据机台名称判断该机台是不是编织机台
        /// </summary>
        /// <param name="machineNo"></param>
        /// <returns></returns>
        public static bool IsBzMachine(string machineNo)
        {
            return !string.IsNullOrEmpty(machineNo) && machineNo.Contains("编织");
        }

        /// <summary>
        /// 根据机台名称判断该机台是不是挤护套机台
        /// </summary>
        /// <param name="machineNo"></param>
        /// <returns></returns>
        public static bool IsJhtMachine(string machineNo)
        {
            return !string.IsNullOrEmpty(machineNo) && machineNo.Contains("90");
        }
    }
}
