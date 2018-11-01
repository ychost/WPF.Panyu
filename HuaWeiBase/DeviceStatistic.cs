using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiBase
{
    /// <summary>
    /// 设备模块的统计信息
    /// </summary>
    public class DeviceStatistic
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public double MaximumValue { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public double MinimumValue { get; set; }
        /// <summary>
        /// 平均值
        /// </summary>
        public double AverageValue { get; set; }
        /// <summary>
        /// 异常次数
        /// </summary>
        public int ErrorTimes { get; set; }

        public DeviceStatistic()
        {
            DeviceName = null;
            MaximumValue = double.MinValue;
            MinimumValue = double.MaxValue;
            AverageValue = 0.0;
            ErrorTimes = 0;
        }
    }
}
