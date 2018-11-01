using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiUtils
{
    public class StringUtil
    {
        /// <summary>
        /// 是否存在不可见字符
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static bool ExistInvisibleChar(string hexString)
        {
            if(string.IsNullOrEmpty(hexString) || hexString.Length % 2 != 0) { return true; }

            bool bResult = false;
            int stringLength = hexString.Length / 2;
            for(int i = 0; i < stringLength; i++)
            {
                try
                {
                    int result = Convert.ToInt32(hexString.Substring(i * 2, 2), 16);
                    // 0-31为控制字符，32-126为打印字符，127为删除字符(delete),128-255为扩展字符
                    if(result < 32 || result > 126) { bResult = true; break; }
                }
                catch { bResult = true; }
            }
            return bResult;
        }

        /// <summary>
        /// 将一串Hex字符解析成ASCII编码的字符串
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="onlyVisibleChar">是否只转换可见字符</param>
        /// <returns></returns>
        public static string GetStringFromHex(string sourceString, bool onlyVisibleChar = false)
        {
            if(string.IsNullOrEmpty(sourceString) || sourceString.Length % 2 != 0)
            {
                return string.Empty;
            }

            int len = sourceString.Length / 2;
            char[] chars = new char[len];
            if(onlyVisibleChar)
            {
                int chartCount = 0;
                for(int i = 0; i < len; i++)
                {
                    try
                    {
                        int result = Convert.ToInt32(sourceString.Substring(i * 2, 2), 16);
                        // 0-31为控制字符，32-126为打印字符，127为删除字符(delete),128-255为扩展字符
                        if(result > 31 && result < 127)
                        {
                            chars[chartCount++] = (char)result;
                        }
                    }
                    catch { return string.Empty; }
                }
                return new string(chars, 0, chartCount);
            }
            else
            {
                for(int i = 0; i < len; i++)
                {
                    try
                    {
                        chars[i] = (char)Convert.ToInt32(sourceString.Substring(i * 2, 2), 16);
                    }
                    catch { return string.Empty; }
                }
                return new string(chars);
            }
        }

        /// <summary>
        /// 将字符串转换为Hex
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetHexFromString(string source)
        {
            string result = string.Empty;
            foreach(char c in source)
            {
                result += ((int)c).ToString("X");
            }
            return result;
        }
    }
}
