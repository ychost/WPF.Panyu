using HuaWeiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuaWeiDAL
{
    [Obsolete("已经不需要保存错误数据了")]
    public class OutlierDAL
    {
        public static bool Add(Outlier outlier)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("insert into [Mes].[dbo].[T_Outlier] (MachineName," +
                "ParamName,Value,Upper,Lower,CollectedTime) values('{0}','{1}'," +
                "{2},{3},{4},'{5}')", outlier.MachineName, outlier.ParamName,
                outlier.Value, outlier.Upper, outlier.Lower, outlier.CollectedTime);
//            return DBHelper.ExecuteNonQuery(sql.ToString(), null);
            return false;
        }
    }
}
