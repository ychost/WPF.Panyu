using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiBase
{
    /// <summary>
    /// 数据集因添加、删除、更新改变后手动绑定
    /// </summary>
    public enum BindingReason
    {
        /// <summary>
        /// 一般原因
        /// </summary>
        None,
        /// <summary>
        /// 添加
        /// </summary>
        Add,
        /// <summary>
        /// 更新
        /// </summary>
        Update,
        /// <summary>
        /// 删除
        /// </summary>
        Delete
    }
}
