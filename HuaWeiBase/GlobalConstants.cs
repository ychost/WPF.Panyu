using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuaWeiBase
{
    public class GlobalConstants
    {
        /// <summary>
        /// 最小显示值，大于等于该值的参数才显示
        /// </summary>
        public static double MinDisplayValue = 0.1;

        /// <summary>
        /// 时间格式化字符串
        /// </summary>
        public static string TimeFormat = "HH:mm:ss";

        /// <summary>
        /// 日期时间格式化字符串
        /// </summary>
        public static string DateTimeFormat = "yyy-MM-dd HH:mm:ss";

        /// <summary>
        /// double数据格式化字符串
        /// </summary>
        public static string DoubleFormat = "#0.000";

        /// <summary>
        /// 非正数的替换符号
        /// </summary>
        public static string NonpositiveReplacer = "— —";

        /// <summary>
        /// null或者空对象的替换符号
        /// </summary>
        public static string NullOrEmptyReplacer = "— —";

        /// <summary>
        /// 全局参数的上界值
        /// </summary>
        public static double GlobalUpper = 5000;

        /// <summary>
        /// 全局参数的下界值
        /// </summary>
        public static double GlobalLower = 0.0;

    }
}
