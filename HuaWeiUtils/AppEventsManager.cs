using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiUtils
{
    /// <summary>
    /// 应用程序级的全局事件管理
    /// </summary>
    public class AppEventsManager
    {
        /// <summary>
        /// 更新数据库连接字符串事件
        /// </summary>
        public static event Action OnUpdateConnectionString;
        public static void UpdateConnectionString()
        {
            if(OnUpdateConnectionString != null)
            {
                OnUpdateConnectionString.Invoke();
            }
        }

        /// <summary>
        /// 更新线缆生产参数数据样式
        /// </summary>
        public static event Action OnUpdateDataPatterns;
        public static void UpdateDataPatterns()
        {
            if(OnUpdateDataPatterns != null)
            {
                OnUpdateDataPatterns.Invoke();
            }
        }

        /// <summary>
        /// 更新底层设备模块的信息
        /// </summary>
        public static event Action OnUpdateFirstFloorDevices;
        public static void UpdateFirstFloorDevices()
        {
            if(OnUpdateFirstFloorDevices != null)
            {
                OnUpdateFirstFloorDevices.Invoke();
            }
        }

        /// <summary>
        /// 更新数据服务端的地址和端口号
        /// </summary>
        public static event Action OnUpdateDataServer;
        public static void UpdateDataServer()
        {
            if (OnUpdateDataServer != null)
            {
                OnUpdateDataServer.Invoke();
            }
        }

        /// <summary>
        /// 更新外径上下限
        /// </summary>
        public static event Action OnUpdateOuterDiameterUpperAndLower;
        public static void UpdateOuterDiameterUpperAndLower()
        {
            if (OnUpdateOuterDiameterUpperAndLower != null)
            {
                OnUpdateOuterDiameterUpperAndLower.Invoke();
            }
        }
    }
}
