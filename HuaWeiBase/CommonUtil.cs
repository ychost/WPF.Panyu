using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiBase
{
    internal class CommonUtil
    {
        public static string GetStringFromHex(string sourceString, bool onlyVisiableChart = false)
        {
            if(string.IsNullOrEmpty(sourceString) || sourceString.Length % 2 != 0)
            {
                return string.Empty;
            }

            int len = sourceString.Length / 2;
            char[] chars = new char[len];
            if(onlyVisiableChart)
            {
                int charCount = 0;
                for(int i = 0; i < len; i++)
                {
                    try
                    {
                        int result = Convert.ToInt32(sourceString.Substring(i * 2, 2), 16);
                        // 0-31为控制字符，32-126为打印字符，127为删除字符(delete),128-255为扩展字符
                        if(result > 31 && result < 127)
                        {
                            chars[charCount++] = (char)result;
                        }
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
                return new string(chars, 0, charCount);
            }
            else
            {
                for(int i = 0; i < len; i++)
                {
                    try
                    {
                        chars[i] = (char)Convert.ToInt32(sourceString.Substring(i * 2, 2), 16);
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
                return new string(chars);
            }
        }

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
