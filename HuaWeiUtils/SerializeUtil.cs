using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace HuaWeiUtils
{
    /// <summary>
    /// 对象串行化工具
    /// </summary>
    public class SerializeUtil
    {
        /// <summary>
        /// 序列化对象到指定文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool SerializableObject(string filePath, object o)
        {
            bool bResult = true;
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    new BinaryFormatter().Serialize(fs, o);
                }
            }
            catch { bResult = false; }
            return bResult;
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T DeserializableObject<T>(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    return (T)new BinaryFormatter().Deserialize(fs);
                }
            }
            catch { }
            return default(T);
        }
    }
}
