using HuaWeiModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HuaWeiDAL
{
    public class MachineDAL
    {
        public static List<Machine> SelectAll()
        {
            List<Machine> list = null;
            Machine machine = null;
            string sql = "select * from [Mes].[dbo].[T_Machine]";

            using (SqlDataReader reader = DBHelper.ExecuteSqlReader(sql))
            {
                if (reader != null)
                {
                    list = new List<Machine>();
                    try
                    {
                        while (reader.Read())
                        {
                            machine = new Machine();
                            if (DBNull.Value != reader["MachineID"])
                                machine.MachineID = Convert.ToInt32(reader["MachineID"]);
                            if (DBNull.Value != reader["MachineTypeID"])
                                machine.MachineTypeID = Convert.ToInt32(reader["MachineTypeID"]);
                            if (DBNull.Value != reader["MachineName"])
                                machine.MachineName = reader["MachineName"].ToString();
                            list.Add(machine);
                        }
                    }
                    catch { list.Clear(); list = null; }
                }
            }
            return list;
        }

        /// <summary>
        /// 按给定的查询条件检索Machine数据
        /// </summary>
        /// <param name="whereString"></param>
        /// <returns></returns>
        public static Machine GetMachine(string whereString)
        {
            Machine machineN = null;
            string sql = "select * from [Mes].[dbo].[T_Machine] " + whereString;
            using (SqlDataReader reader = DBHelper.ExecuteSqlReader(sql))
            {
                if (reader != null && reader.Read())
                {
                    try
                    {
                        machineN = new Machine();
                        if (DBNull.Value != reader["MachineID"])
                            machineN.MachineID = Convert.ToInt32(reader["MachineID"]);
                        if (DBNull.Value != reader["MachineTypeID"])
                            machineN.MachineTypeID = Convert.ToInt32(reader["MachineTypeID"]);
                        if (DBNull.Value != reader["MachineName"])
                            machineN.MachineName = reader["MachineName"].ToString();
                    }
                    catch { machineN = null; }
                }
            }
            return machineN;
        }

    }
}
