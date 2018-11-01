using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuaWeiModel
{
    /// <summary>
    /// 生产工艺表
    /// </summary>
    public class Crafwork
    {
        public int Id { get; set; }

        /// <summary>
        /// 工艺编号
        /// </summary>
        public string CrafworkCode { get; set; }

        /// <summary>
        /// 生产工艺文件
        /// </summary>
        public string FileName { get; set; }
    }
}
