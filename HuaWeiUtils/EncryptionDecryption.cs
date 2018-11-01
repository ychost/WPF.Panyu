using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiUtils
{
    public class EncryptionDecryption
    {
        /// <summary>
        /// 密钥
        /// </summary>
        private static string encryptKey = "Wenl";

        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encrypt(string str)
        {
            string encryptString = null;
            try
            {
                //实例化加/解密类对象
                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();

                //定义字节数组，用来存储密钥
                byte[] key = Encoding.Unicode.GetBytes(encryptKey);
                //定义字节数组，用来存储要加密的字符串
                byte[] data = Encoding.Unicode.GetBytes(str);
                //实例化内存流对象
                MemoryStream mStream = new MemoryStream();
                //使用内存流实例化加密流对象   
                CryptoStream CStream = new CryptoStream(mStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
                //向加密流中写入数据
                CStream.Write(data, 0, data.Length);
                //释放加密流
                CStream.FlushFinalBlock();

                //获取加密后的字符串
                encryptString = Convert.ToBase64String(mStream.ToArray());
            }
            catch { }

            return encryptString;
        }

        /// <summary>
        /// 字符串解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decrypt(string str)
        {
            string decryptString = null;
            try
            {
                //实例化加/解密类对象
                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();

                //定义字节数组，用来存储密钥    
                byte[] key = Encoding.Unicode.GetBytes(encryptKey);
                //定义字节数组，用来存储要解密的字符串
                byte[] data = Convert.FromBase64String(str);
                //实例化内存流对象
                MemoryStream mStream = new MemoryStream();

                //使用内存流实例化解密流对象       
                CryptoStream CStream = new CryptoStream(mStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
                //向解密流中写入数据
                CStream.Write(data, 0, data.Length);
                //释放解密流
                CStream.FlushFinalBlock();

                //获取解密后的字符串
                decryptString = Encoding.Unicode.GetString(mStream.ToArray());
            }
            catch { }

            return decryptString;
        }
    }
}
