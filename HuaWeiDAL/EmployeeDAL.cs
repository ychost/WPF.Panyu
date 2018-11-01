using HuaWeiModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HuaWeiDAL
{
    public class EmployeeDAL
    {
        public static List<Employee> SelectAll()
        {
            List<Employee> list = null;
            Employee employee = null;
            string sql = "select * from [Mes].[dbo].[T_Employee]";

            using (SqlDataReader reader = DBHelper.ExecuteSqlReader(sql))
            {
                if (reader != null)
                {
                    list = new List<Employee>();
                    try
                    {
                        while (reader.Read())
                        {
                            employee = new Employee();
                            if (DBNull.Value != reader["EmployeeID"])
                                employee.EmployeeID = Convert.ToInt32(reader["EmployeeID"]);
                            if (DBNull.Value != reader["EmployeeCode"])
                                employee.EmployeeCode = reader["EmployeeCode"].ToString();
                            if (DBNull.Value != reader["EmployeeName"])
                                employee.EmployeeName = reader["EmployeeName"].ToString();
                            list.Add(employee);
                        }
                    }
                    catch { list.Clear(); list = null; }
                }
            }
            return list;
        }

        /// <summary>
        /// 按给定的查询条件检索EmployeeN数据
        /// </summary>
        /// <param name="whereString"></param>
        /// <returns></returns>
        public static Employee GetEmployee(string whereString)
        {
            Employee employee = null;
            string sql = "select * from [Mes].[dbo].[T_Employee] " + whereString;
            using (SqlDataReader reader = DBHelper.ExecuteSqlReader(sql))
            {
                if (reader != null && reader.Read())
                {
                    try
                    {
                        employee = new Employee();
                        if (DBNull.Value != reader["EmployeeID"])
                            employee.EmployeeID = Convert.ToInt32(reader["EmployeeID"]);
                        if (DBNull.Value != reader["EmployeeCode"])
                            employee.EmployeeCode = reader["EmployeeCode"].ToString();
                        if (DBNull.Value != reader["EmployeeName"])
                            employee.EmployeeName = reader["EmployeeName"].ToString();
                    }
                    catch { employee = null; }
                }
            }
            return employee;
        }
    }
}
