using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuaWeiModel
{
    /// <summary>
    /// 生产计划
    /// 扫描后会获取到生产计划工单的
    /// </summary>
    public class ProductPlan
    {
        public int Id { get; set; }

        /// <summary>
        /// 外键，关联到Crafwork(工艺)的id
        /// </summary>
        public int Mid { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkSheetNo { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string MaterialNo { get; set; }

        /// <summary>
        /// 物料颜色
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 出货长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Bak { get; set; }

        /// <summary>
        /// 排产长度
        /// </summary>
        public int ArrLength { get; set; }
    }
}
