using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HuaWeiUtils
{
    public class XmlUtil
    {
        public static readonly string AppConfig = @"Config\AppConfig.xml";
        public static readonly string FirstFloorDevice = @"Config\FirstFloorDevice.xml";

        /// <summary>
        /// 通过xsd验证xml格式是否正确，正确返回空字符串，错误返回提示
        /// </summary>
        /// <param name="xmlFile">xml文件</param>
        /// <param name="xsdFile">xsd文件</param>
        /// <param name="namespaceUrl">命名空间，无则默认为null</param>
        /// <returns>正确返回空字符串，错误返回提示</returns>
        public static string XmlValidationByXsd(string xmlFile, string xsdFile, string namespaceUrl = null)
        {
            StringBuilder sb = new StringBuilder();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(namespaceUrl, xsdFile);
            settings.ValidationEventHandler += (sender, arg) =>
            {
                sb.AppendFormat("{0}|", arg.Message);
            };

            using (XmlReader reader = XmlReader.Create(xmlFile, settings))
            {
                try
                {
                    while (reader.Read())
                        ;
                }
                catch (XmlException ex)
                {
                    sb.AppendFormat("{0}|", ex.Message);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 从指定Xml文件中获取指定XPath元素的InnerText集合
        /// </summary>
        /// <param name="xmlFile">XML文件路径</param>
        /// <param name="xPath">指定查找的XPath</param>
        /// <returns></returns>
        public static List<string> XmlGetElementTexts(string xmlFile, string xPath)
        {
            List<string> texts = new List<string>();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFile);
                XmlElement root = doc.DocumentElement;
                XmlNodeList listNodes = root.SelectNodes(xPath);
                foreach (XmlNode node in listNodes)
                {
                    texts.Add(node.InnerText);
                }
            }
            catch
            {
                return null;
            }
            return texts;
        }

        /// <summary>
        /// 从指定Xml文件中获取指定XPath下的第一个元素的InnerText，如果没有元素则返回空字符串
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static string XmlGetElementText(string xmlFile, string xPath)
        {
            List<string> texts = XmlGetElementTexts(xmlFile, xPath + "[1]");
            if (texts != null && texts.Count > 0)
            {
                return texts[0];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 从指定XML文件中获取T对象列表
        /// </summary>
        /// <typeparam name="T">数据模型的类型</typeparam>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns></returns>
        public static List<T> GetModelFromXmlFile<T>(string xmlPath) where T : new()
        {
            Type type = new T().GetType();

            // 获取该类型名称，其作为解析XML时筛选节点的XPath
            string className = type.ToString().Split('.').Last();

            // 获取类型的所有属性及其数据类型
            PropertyInfo[] props = type.GetProperties();
            List<string> propNames = new List<string>();
            List<Type> propTypes = new List<Type>();
            foreach (var prop in props)
            {
                propNames.Add(prop.Name);
                propTypes.Add(prop.PropertyType);
            }

            List<T> dataModelList = new List<T>();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlPath);
                XmlElement root = doc.DocumentElement;
                XmlNodeList listNodes = root.SelectNodes(className);

                int count = listNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    T t = new T();
                    XmlNode node = listNodes[i];

                    // 在当前节点的属性中查找实体类的属性
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        if (propNames.Contains(attr.Name))
                        {
                            object value = BaseTypeConvertor(attr.Value, propTypes[propNames.IndexOf(attr.Name)]);
                            type.InvokeMember(attr.Name, BindingFlags.SetProperty | BindingFlags.Public
                                | BindingFlags.Instance, null, t, new object[] { value });
                        }
                    }

                    // 在当前节点的子节点中查找实体类的属性
                    XmlNodeList subNodes = root.SelectNodes(className + "[" + (i + 1) + "]/*");
                    foreach (XmlNode subNode in subNodes)
                    {
                        if (propNames.Contains(subNode.Name))
                        {
                            object value = BaseTypeConvertor(subNode.InnerText, propTypes[propNames.IndexOf(subNode.Name)]);
                            type.InvokeMember(subNode.Name, BindingFlags.SetProperty | BindingFlags.Public
                                | BindingFlags.Instance, null, t, new object[] { value });
                        }
                    }
                    dataModelList.Add(t);
                }
            }
            catch { }

            return dataModelList;
        }

        /// <summary>
        /// 从指定的XML字符串中获取T对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static List<T> GetModelFromXmlString<T>(string xmlString) where T : new()
        {
            Type type = new T().GetType();

            // 获取该类型名称，其作为解析XML时筛选节点的XPath
            string className = type.ToString().Split('.').Last();

            // 获取类型的所有属性及其数据类型
            PropertyInfo[] props = type.GetProperties();
            List<string> propNames = new List<string>();
            List<Type> propTypes = new List<Type>();
            foreach (var prop in props)
            {
                propNames.Add(prop.Name);
                propTypes.Add(prop.PropertyType);
            }

            List<T> dataModelList = new List<T>();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlString);
                XmlElement root = doc.DocumentElement;
                XmlNodeList listNodes = root.SelectNodes(className);

                int count = listNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    T t = new T();
                    XmlNode node = listNodes[i];

                    // 在当前节点的属性中查找实体类的属性
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        if (propNames.Contains(attr.Name))
                        {
                            object value = BaseTypeConvertor(attr.Value, propTypes[propNames.IndexOf(attr.Name)]);
                            type.InvokeMember(attr.Name, BindingFlags.SetProperty | BindingFlags.Public
                                | BindingFlags.Instance, null, t, new object[] { value });
                        }
                    }

                    // 在当前节点的子节点中查找实体类的属性
                    XmlNodeList subNodes = root.SelectNodes(className + "[" + (i + 1) + "]/*");
                    foreach (XmlNode subNode in subNodes)
                    {
                        if (propNames.Contains(subNode.Name))
                        {
                            object value = BaseTypeConvertor(subNode.InnerText, propTypes[propNames.IndexOf(subNode.Name)]);
                            type.InvokeMember(subNode.Name, BindingFlags.SetProperty | BindingFlags.Public
                                | BindingFlags.Instance, null, t, new object[] { value });
                        }
                    }
                    dataModelList.Add(t);
                }
            }
            catch { }

            return dataModelList;
        }

        /// <summary>
        /// 将字符串转换为其他基本数据类型
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        public static object BaseTypeConvertor(string source, Type type)
        {
            try
            {
                string typeName = type.ToString();
                if ("System.Double".Equals(typeName))
                {
                    return double.Parse(source);
                }
                else if ("System.Int32".Equals(typeName))
                {
                    return int.Parse(source);
                }
                else if ("System.Single".Equals(typeName))
                {
                    return float.Parse(source);
                }
                else if ("System.String".Equals(typeName))
                {
                    return source;
                }
                else
                {
                    return null;
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 修改Xml文档中指定标签的节点的值
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="tagName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool ChangeXmlByTagName(string xmlPath, string tagName, string newValue)
        {
            bool result = false;
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(xmlPath);
                XmlNodeList nodes = xml.GetElementsByTagName(tagName);
                if (nodes != null && nodes.Count > 0)
                {
                    nodes[0].InnerText = newValue;
                    xml.Save(xmlPath);
                }
                result = true;
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 修改Xml文档中指定标签的节点的值
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="tagName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool ChangeXmlByTagNames(string xmlPath, IEnumerable<string> tagNames,
            IEnumerable<string> newValues)
        {
            bool result = false;
            if (tagNames != null && newValues != null && tagNames.Count() == newValues.Count())
            {
                XmlDocument xml = new XmlDocument();
                try
                {
                    xml.Load(xmlPath);
                    IEnumerator enumerator1 = tagNames.GetEnumerator();
                    IEnumerator enumerator2 = newValues.GetEnumerator();
                    while (enumerator1.MoveNext() && enumerator2.MoveNext())
                    {
                        string tagName = Convert.ToString(enumerator1.Current);
                        string newValue = Convert.ToString(enumerator2.Current);

                        XmlNodeList nodes = xml.GetElementsByTagName(tagName);
                        if (nodes != null && nodes.Count > 0)
                        {
                            nodes[0].InnerText = newValue;
                        }
                    }
                    xml.Save(xmlPath);
                    result = true;
                }
                catch { }
            }
            return result;
        }
    }
}
