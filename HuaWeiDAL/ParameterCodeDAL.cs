using HuaWeiModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HuaWeiDAL
{
    public class ParameterCodeDAL
    {
        public static List<ParameterCode> SelectAll()
        {
            List<ParameterCode> list = null;
            ParameterCode parameterCode = null;
            string sql = "select * from [Mes].[dbo].[T_ParameterCode] order by Type";

            using (SqlDataReader reader = DBHelper.ExecuteSqlReader(sql))
            {
                if (reader != null)
                {
                    list = new List<ParameterCode>();
                    try
                    {
                        while (reader.Read())
                        {
                            parameterCode = new ParameterCode();
                            if (DBNull.Value != reader["Type"])
                                parameterCode.ParameterTypeId = Convert.ToInt32(reader["Type"]);
                            if (DBNull.Value != reader["ParameterName"])
                                parameterCode.ParameterName = reader["ParameterName"].ToString();
                            list.Add(parameterCode);
                        }
                    }
                    catch { list.Clear(); list = null; }
                }
            }
            return list;
        }

        /// <summary>
        /// 按给定的查询条件检索ParameterCode数据
        /// </summary>
        /// <param name="whereString"></param>
        /// <returns></returns>
        public static ParameterCode GetParameterCode(string whereString)
        {
            ParameterCode parameterCode = null;
            string sql = "select * from [Mes].[dbo].[T_ParameterCode] " + whereString;
            using (SqlDataReader reader = DBHelper.ExecuteSqlReader(sql))
            {
                if (reader != null && reader.Read())
                {
                    try
                    {
                        parameterCode = new ParameterCode();
                        if (DBNull.Value != reader["Type"])
                            parameterCode.ParameterTypeId = Convert.ToInt32(reader["Type"]);
                        if (DBNull.Value != reader["ParameterName"])
                            parameterCode.ParameterName = reader["ParameterName"].ToString();
                    }
                    catch { parameterCode = null; }
                }
            }
            return parameterCode;
        }
    }
}
