using HuaWeiModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HuaWeiDAL
{
    public class CrafworkDAL
    {
        public static Crafwork GetCrafwork(string whereString)
        {
            Crafwork crafwork = null;
            string sql = "select * from [Mes].[dbo].[T_crafwork] " + whereString;

            using (SqlDataReader reader = DBHelper.ExecuteSqlReader(sql))
            {
                if (reader != null && reader.Read())
                {
                    try
                    {
                        crafwork = new Crafwork();
                        if (DBNull.Value != reader["id"])
                            crafwork.Id = Convert.ToInt32(reader["id"]);
                        if (DBNull.Value != reader["crafworkcode"])
                            crafwork.CrafworkCode = reader["crafworkcode"].ToString();
                        if (DBNull.Value != reader["filename"])
                            crafwork.FileName = reader["filename"].ToString();
                    }
                    catch { crafwork = null; }
                }
            }
            return crafwork;
        }
    }
}
