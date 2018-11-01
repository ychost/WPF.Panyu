using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiBase
{
    /// <summary>
    /// 保存机台的信息
    /// </summary>
    public class MachineInfo
    {
        /// <summary>
        /// 机台名称
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// 机台编号
        /// </summary>
        public string MachineNo { get; set; }

        /// <summary>
        /// 所处工序名称
        /// </summary>
        public string ProcessName { get; set; }
    }
}
