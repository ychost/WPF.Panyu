using HuaWeiBase;
using HuaWeiModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace HuaWeiUtils
{
    public class Configuration
    {
        public static readonly string AppConfig = @"Config\AppConfig.xml";

        /// <summary>
        /// 窗口主标题
        /// </summary>
        public static string Title { get; private set; }

        /// <summary>
        /// 窗口子标题
        /// </summary>
        public static string SubTitle { get; private set; }

        /// <summary>
        /// 关闭窗口后退出还是最小化到系统托盘
        /// </summary>
        public static bool CloseMainwindowToExit { get; private set; }

        /// <summary>
        /// 连接数据库的相关参数
        /// </summary>
        /// ********************************************************************/
        /// <summary>
        /// 数据源地址
        /// </summary>
        public static string DataSource { get; private set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public static string InitialCatalog { get; private set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserId { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        public static string Password { get; private set; }
        /// ********************************************************************/

        /// <summary>
        /// 连接数据服务端的参数
        /// </summary>
        /// ********************************************************************/
        /// <summary>
        /// 数据中心的Ip地址
        /// </summary>
        public static string ServerIp { get; private set; }

        /// <summary>
        /// 连接到数据中心服务端的端口号
        /// </summary>
        public static string ServerPort { get; private set; }
        /// ********************************************************************/

        /// <summary>
        /// 外径上限值
        /// </summary>
        public static double OuterDiameterUpper { get; set; }

        /// <summary>
        /// 外径下限值
        /// </summary>
        public static double OuterDiameterLower { get; set; }

        /// <summary>
        /// 默认的机台名称
        /// </summary>
        public static string DefaultMachine { get; private set; }

        public static ConfigurationDto ConfigurationDto { get; private set; }

        /// <summary>
        /// 所有机台对象
        /// </summary>
        public static Dictionary<string, Machine> MachineMap { get; set; }

        /// <summary>
        /// 所有员工对象
        /// </summary>
        public static Dictionary<string, Employee> EmployeeMap { get; set; }

        /// <summary>
        /// 所有的采集参数对象
        /// 索引为ParameterCode的Type值
        /// 值为ParameterCode的ParameterName
        /// </summary>
        public static string[] ParameterCodeArray { get; set; }

        static Configuration()
        {
            ConfigurationDto = new ConfigurationDto();

            /// Title、SubTitle、CloseMainwindowToExit
            Title = XmlUtil.XmlGetElementText(AppConfig,
                "/Application/Window[@name='MainWindow']/Title");

            SubTitle = XmlUtil.XmlGetElementText(
                AppConfig, "/Application/Window[@name='MainWindow']/SubTitle");

            bool closeMainwindowToExit;
            bool.TryParse(XmlUtil.XmlGetElementText(AppConfig,
                "/Application/CloseMainwindowToExit"), out closeMainwindowToExit);
            CloseMainwindowToExit = closeMainwindowToExit;
            ConfigurationDto.CloseMainwindowToExit = closeMainwindowToExit;

            /// DataBaseParam
            ConfigurationDto.DataSource = DataSource = XmlUtil.XmlGetElementText(
                AppConfig, "/Application/ConnectionString/DataSource");
            ConfigurationDto.InitialCatalog = InitialCatalog = XmlUtil.XmlGetElementText(
                AppConfig, "/Application/ConnectionString/InitialCatalog");
            ConfigurationDto.UserId = UserId = XmlUtil.XmlGetElementText(
                AppConfig, "/Application/ConnectionString/UserId");

            string encryptPassword = XmlUtil.XmlGetElementText(AppConfig,
                "/Application/ConnectionString/Password");
            Password = EncryptionDecryption.Decrypt(encryptPassword);
            ConfigurationDto.Password = Password;

            /// DataServerParam
            ConfigurationDto.ServerIp = ServerIp = XmlUtil.XmlGetElementText(
                AppConfig, "/Application/DataServer/DataServerIP");
            ConfigurationDto.ServerPort = ServerPort = XmlUtil.XmlGetElementText(
                AppConfig, "/Application/DataServer/DataServerPort");

            string encryptSpecPassword = XmlUtil.XmlGetElementText(
                AppConfig, "/Application/SpecPassword");
            DefaultMachine = XmlUtil.XmlGetElementText(AppConfig,
                "/Application/DefaultMachine/@name");

            double outerDiameter = 0.0;
            double.TryParse(XmlUtil.XmlGetElementText(AppConfig,
                "/Application/OuterDiameter/Upper"), out outerDiameter);
            ConfigurationDto.OuterDiameterUpper = OuterDiameterUpper = outerDiameter;

            double.TryParse(XmlUtil.XmlGetElementText(AppConfig,
                "/Application/OuterDiameter/Lower"), out outerDiameter);
            ConfigurationDto.OuterDiameterLower = OuterDiameterLower = outerDiameter;

            MachineMap = new Dictionary<string, Machine>();
            EmployeeMap = new Dictionary<string, Employee>();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <returns></returns>
        public static string SaveConfiguration()
        {
            // Validate
            if (!ValidateUtil.ValidateIp(ConfigurationDto.DataSource))
                return "连接数据库的数据源地址格式有误！";
            if (!ValidateUtil.ValidateIp(ConfigurationDto.ServerIp))
                return "连接数据中心的地址格式有误！";
            if (!ValidateUtil.ValidatePort(ConfigurationDto.ServerPort))
                return "连接数据中心的端口设置有误！";

            bool configParameterBlured = false;
            List<string> tagNames = new List<string>();
            List<string> newValues = new List<string>();

            /// CloseMainwindowToExit
            if (CloseMainwindowToExit != ConfigurationDto.CloseMainwindowToExit)
            {
                tagNames.Add("CloseMainwindowToExit");
                newValues.Add(ConfigurationDto.CloseMainwindowToExit.ToString());
                CloseMainwindowToExit = ConfigurationDto.CloseMainwindowToExit;
            }

            /// DataBase Parameters
            if (!DataSource.Equals(ConfigurationDto.DataSource))
            {
                configParameterBlured = true;
                tagNames.Add("DataSource");
                newValues.Add(ConfigurationDto.DataSource);
                DataSource = ConfigurationDto.DataSource;
            }
            if (!InitialCatalog.Equals(ConfigurationDto.InitialCatalog))
            {
                configParameterBlured = true;
                tagNames.Add("InitialCatalog");
                newValues.Add(ConfigurationDto.InitialCatalog);
                InitialCatalog = ConfigurationDto.InitialCatalog;
            }
            if (!UserId.Equals(ConfigurationDto.UserId))
            {
                configParameterBlured = true;
                tagNames.Add("UserId");
                newValues.Add(ConfigurationDto.UserId);
                UserId = ConfigurationDto.UserId;
            }
            if (!Password.Equals(ConfigurationDto.Password))
            {
                configParameterBlured = true;
                tagNames.Add("Password");
                newValues.Add(EncryptionDecryption.Encrypt(ConfigurationDto.Password));
                Password = ConfigurationDto.Password;
            }
            if (configParameterBlured)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                {
                    AppEventsManager.UpdateConnectionString();
                }), null);
            }

            configParameterBlured = false;
            /// Data Server Parameter
            if (!ServerIp.Equals(ConfigurationDto.ServerIp))
            {
                configParameterBlured = true;
                tagNames.Add("DataServerIP");
                newValues.Add(ConfigurationDto.ServerIp);
                ServerIp = ConfigurationDto.ServerIp;
            }
            if (!ServerPort.Equals(ConfigurationDto.ServerPort))
            {
                configParameterBlured = true;
                tagNames.Add("DataServerPort");
                newValues.Add(ConfigurationDto.ServerPort);
                ServerPort = ConfigurationDto.ServerPort;
            }
            if (configParameterBlured)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                {
                    AppEventsManager.UpdateDataServer();
                }), null);
            }

            configParameterBlured = false;
            if (OuterDiameterUpper != ConfigurationDto.OuterDiameterUpper)
            {
                tagNames.Add("Upper"); 
                configParameterBlured = true;
                newValues.Add("" + ConfigurationDto.OuterDiameterUpper);
                OuterDiameterUpper = ConfigurationDto.OuterDiameterUpper;
            }

            if (OuterDiameterLower != ConfigurationDto.OuterDiameterLower)
            {
                tagNames.Add("Lower");
                configParameterBlured = true;
                newValues.Add("" + ConfigurationDto.OuterDiameterLower);
                OuterDiameterLower = ConfigurationDto.OuterDiameterLower;
            }

            if (configParameterBlured)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                {
                    AppEventsManager.UpdateOuterDiameterUpperAndLower();
                }), null);
            }


            if (tagNames.Count > 0)
                XmlUtil.ChangeXmlByTagNames(AppConfig, tagNames, newValues);

            return null;
        }

        /// <summary>
        /// 恢复配置
        /// </summary>
        public static void RecoveryConfiguration()
        {
            /// CloseMainwindowToExit
            ConfigurationDto.CloseMainwindowToExit = CloseMainwindowToExit;

            /// DataBase Parameters
            ConfigurationDto.DataSource = DataSource;
            ConfigurationDto.InitialCatalog = InitialCatalog;
            ConfigurationDto.UserId = UserId;
            ConfigurationDto.Password = Password;

            /// Data Server Parameter
            ConfigurationDto.ServerIp = ServerIp;
            ConfigurationDto.ServerPort = ServerPort;

            ConfigurationDto.OuterDiameterUpper = OuterDiameterUpper;
            ConfigurationDto.OuterDiameterLower = OuterDiameterLower;
        }
    }
}
