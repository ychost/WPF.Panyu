using HuaWeiModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HuaWeiDAL
{
    public class MaterialDAL
    {
        /// <summary>
        /// 按给定的查询条件检索MaterialN数据
        /// </summary>
        /// <param name="whereString"></param>
        /// <returns></returns>
        public static Material GetMaterial(string whereString)
        {
            Material material = null;
            string sql = "select * from [Mes].[dbo].[T_MaterialOutput] " + whereString;
            using (SqlDataReader reader = DBHelper.ExecuteSqlReader(sql))
            {
                if (reader != null && reader.Read())
                {
                    try
                    {
                        material = new Material();
                        if (DBNull.Value != reader["MaterialOutputID"])
                            material.MaterialOutputID = Convert.ToInt32(reader["MaterialOutputID"]);
                        if (DBNull.Value != reader["MaterialRFID"])
                            material.MaterialRfid = reader["MaterialRFID"].ToString();
                        if (DBNull.Value != reader["Color"])
                            material.MaterialColor = reader["Color"].ToString();
                        if (DBNull.Value != reader["MaterialName"])
                            material.MaterialName = reader["MaterialName"].ToString();
                    }
                    catch { material = null; }
                }
            }
            return material;
        }
    }
}
