using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuaWeiUtils
{
    public class ValidateUtil
    {
        /// <summary>
        /// 校验IP地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool ValidateIp(string ip)
        {
            if (string.IsNullOrEmpty(ip)) { return false; }

            string[] ips = ip.Split('.');
            if (ips != null && ips.Length == 4)
            {
                int value;
                for (int i = 0; i < 4; i++)
                {
                    if (!int.TryParse(ips[i], out value)) { return false; }
                    if (value < 0 || value > 255) { return false; }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 校验端口号
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool ValidatePort(string port)
        {
            if (string.IsNullOrEmpty(port)) { return false; }

            int value;
            if (!int.TryParse(port, out value)) { return false; }

            if (value <= 1024 || value > 65535) { return false; }

            return true;
        }
    }
}
