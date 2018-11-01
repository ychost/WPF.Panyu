using HuaWeiBase;
using HuaWeiUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfHuaWei.Utils
{
    public class CableTypeUtil
    {
        /// <summary>
        /// 获取某一工序中具体的一种线材的参数
        /// </summary>
        /// <param name="cableName"></param>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static CableType GetCableType(string cableName, string processName)
        {
            CableType cableType = null;
            string filePath = @"Config\CableTypes\" + processName + "\\" + cableName + ".xml";
            if(File.Exists(filePath))
            {
                cableType = new CableType();
                cableType.Name = cableName;
                cableType.ProcessName = processName;
                cableType.Patterns = XmlUtil.GetModelFromXmlFile<DataPattern>(filePath);
            }
            return cableType;
        }

        /// <summary>
        /// 读取某一工序所有线材的参数信息
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static List<CableType> GetCableTypes(string processName)
        {
            List<CableType> cableTypes = new List<CableType>();
            string dirPath = @"Config\CableTypes\" + processName;
            if(!Directory.Exists(dirPath))
            {
                try
                {
                    Directory.CreateDirectory(dirPath);
                }
                catch
                {
                    return cableTypes;
                }
            }

            string[] files = Directory.GetFiles(dirPath, "*.xml", SearchOption.TopDirectoryOnly);
            foreach(string filePath in files)
            {
                try
                {
                    CableType cableType = new CableType();
                    cableType.Name = filePath.Split('\\').Last().Split('.').First();
                    cableType.ProcessName = processName;

                    cableType.Patterns = XmlUtil.GetModelFromXmlFile<DataPattern>(filePath);
                    if(cableType.Patterns != null && cableType.Patterns.Count > 0)
                    {
                        cableTypes.Add(cableType);
                    }
                }
                catch { }
            }
            return cableTypes;
        }

        /// <summary>
        /// 保存某一道工序的所有线材参数信息
        /// </summary>
        /// <param name="cableTypes"></param>
        /// <returns></returns>
        public static string SaveCableTypes(List<CableType> cableTypes)
        {
            string resultMsg = string.Empty;
            if(cableTypes != null && cableTypes.Count > 0)
            {
                string dirPath = dirPath = @"Config\CableTypes\" + cableTypes[0].ProcessName;
                if(!Directory.Exists(dirPath))
                {
                    try
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    catch(Exception e)
                    {
                        return e.Message;
                    }
                }
                else
                {
                    string[] files = Directory.GetFiles(dirPath);
                    try
                    {
                        foreach(string file in files)
                        {
                            File.Delete(file);
                        }
                    }
                    catch { }
                }

                foreach(CableType cable in cableTypes)
                {
                    string filePath = dirPath + "\\" + cable.Name + ".xml";
                    string xmlString = "<DataPatterns>\r\n";
                    foreach(DataPattern pattern in cable.Patterns)
                    {
                        pattern.Minimum = pattern.Lower * 2 - pattern.Upper;
                        if(pattern.Minimum < 0)
                        {
                            pattern.Minimum = 0;
                            pattern.Maximum = pattern.Upper / 4.0 * 5.0;
                        }
                        else
                        {
                            pattern.Maximum = pattern.Upper + (pattern.Upper - pattern.Lower) / 2.0;
                        }
                        pattern.Interval = (pattern.Maximum - pattern.Minimum) / 5.0;

                        xmlString += "\t<DataPattern Name=\"" + pattern.Name + "\">\r\n";
                        xmlString += "\t\t<DBFieldName>" + pattern.DBFieldName + "</DBFieldName>\r\n";
                        xmlString += "\t\t<Maximum>" + pattern.Maximum + "</Maximum>\r\n";
                        xmlString += "\t\t<Minimum>" + pattern.Minimum + "</Minimum>\r\n";
                        xmlString += "\t\t<Upper>" + pattern.Upper + "</Upper>\r\n";
                        xmlString += "\t\t<Lower>" + pattern.Lower + "</Lower>\r\n";
                        xmlString += "\t\t<Suffix>" + pattern.Suffix + "</Suffix>\r\n";
                        xmlString += "\t\t<Interval>" + pattern.Interval + "</Interval>\r\n";

                        xmlString += "\t</DataPattern>\r\n";
                    }
                    xmlString += "</DataPatterns>";

                    StreamWriter streamWriter = null;
                    try
                    {
                        streamWriter = new StreamWriter(filePath, false, Encoding.GetEncoding("utf-8"));
                        streamWriter.Write(xmlString);
                    }
                    catch(Exception e)
                    {
                        resultMsg = e.Message;
                    }
                    finally
                    {
                        if(streamWriter != null)
                        {
                            streamWriter.Close();
                        }
                    }
                }
            }
            return resultMsg;
        }
    }
}
