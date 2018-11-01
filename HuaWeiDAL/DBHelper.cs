using HuaWeiUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HuaWeiDAL
{
    /// <summary>
    /// 数据库连接的帮助类
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string connString = string.Empty;

        static DBHelper()
        {
            AppEventsManager_OnUpdateConnectionString();
            AppEventsManager.OnUpdateConnectionString += AppEventsManager_OnUpdateConnectionString;
        }

        private static void AppEventsManager_OnUpdateConnectionString()
        {
            // 获取连接字符串
            string baseXPath = "/Application/ConnectionString/";
            string dataSource = XmlUtil.XmlGetElementText(XmlUtil.AppConfig, baseXPath + "DataSource");
            string catalog = XmlUtil.XmlGetElementText(XmlUtil.AppConfig, baseXPath + "InitialCatalog");
            string userId = XmlUtil.XmlGetElementText(XmlUtil.AppConfig, baseXPath + "UserId");
            string password = XmlUtil.XmlGetElementText(XmlUtil.AppConfig, baseXPath + "Password");

            password = EncryptionDecryption.Decrypt(password);
            connString = "Data Source=" + dataSource + ";Initial Catalog=" + catalog + ";User Id=" + userId + ";Password=" + password + ";";
        }

        /// <summary>
        /// 查询数据库中的数据
        /// </summary>
        /// <param name="sql">执行查询的sql语句</param>
        /// <returns>查询结果的读取器</returns>
        public static SqlDataReader ExecuteSqlReader(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 2000;
            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                try
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                catch { }
            }

            return null;
        }

        /// <summary>
        /// 查询数据，并返回读取器对象
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string cmdText, SqlParameter[] cmdParms)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 2000;
            try
            {
                PrepareCommand(conn, cmd, cmdText, cmdParms);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return dr;
            }
            catch
            {
                try
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                catch { }
            }

            return null;
        }

        public static bool ExecuteNonQuery(string cmdText, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 2000;
            try
            {
                PrepareCommand(conn, cmd, cmdText, param);
                if (param == null)
                    cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return true;
            }
            catch
            {
                try
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// 设置Command参数
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        private static void PrepareCommand(SqlConnection conn, SqlCommand cmd,
            string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = 2000;

            cmd.CommandType = CommandType.StoredProcedure;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}
