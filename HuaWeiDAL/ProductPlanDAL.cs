using HuaWeiModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HuaWeiDAL
{
    public class ProductPlanDAL
    {
        public static ProductPlan GetProductPlan(string whereString)
        {
            ProductPlan productPlan = null;
            string sql = "select * from [Mes].[dbo].[T_Plan] " + whereString;

            using (SqlDataReader reader = DBHelper.ExecuteSqlReader(sql))
            {
                if (reader != null && reader.Read())
                {
                    try
                    {
                        productPlan = new ProductPlan();
                        if (DBNull.Value != reader["id"])
                            productPlan.Id = Convert.ToInt32(reader["id"]);
                        if (DBNull.Value != reader["mid"])
                            productPlan.Mid = Convert.ToInt32(reader["mid"]);
                        if (DBNull.Value != reader["worksheetno"])
                            productPlan.WorkSheetNo = reader["worksheetno"].ToString();
                        if (DBNull.Value != reader["materialno"])
                            productPlan.MaterialNo = reader["materialno"].ToString();
                        if (DBNull.Value != reader["color"])
                            productPlan.Color = reader["color"].ToString();
                        if (DBNull.Value != reader["specification"])
                            productPlan.Specification = reader["specification"].ToString();
                        if (DBNull.Value != reader["length"])
                            productPlan.Length = Convert.ToInt32(reader["length"]);
                        if (DBNull.Value != reader["bak"])
                            productPlan.Bak = reader["bak"].ToString();
                        if (DBNull.Value != reader["arrlength"])
                            productPlan.ArrLength = Convert.ToInt32(reader["arrlength"]);
                    }
                    catch { productPlan = null; }
                }
            }
            return productPlan;
        }

    }
}
