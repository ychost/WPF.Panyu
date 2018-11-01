using HuaWeiBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace HuaWeiUtils
{
    public class ConfigurationDto : INotifyPropertyChanged
    {
        /// <summary>
        /// 关闭窗口后退出还是最小化到系统托盘
        /// </summary>
        private bool closeMainwindowToExit;
        public bool CloseMainwindowToExit
        {
            get { return closeMainwindowToExit; }
            set
            {
                if (value != closeMainwindowToExit)
                {
                    closeMainwindowToExit = value;
                    OnPropertyChanged("CloseMainwindowToExit");
                }
            }
        }

        /// <summary>
        /// 连接数据库的相关参数
        /// </summary>
        /// ********************************************************************/
        /// <summary>
        /// 数据源地址
        /// </summary>
        private string dataSource;
        public string DataSource
        {
            get { return dataSource; }
            set
            {
                if (!string.IsNullOrEmpty(value)
                    && !value.Equals(dataSource))
                {
                    dataSource = value;
                    OnPropertyChanged("DataSource");
                }
            }
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        private string initialCatalog;
        public string InitialCatalog
        {
            get { return initialCatalog; }
            set
            {
                if (!string.IsNullOrEmpty(value)
                    && !value.Equals(initialCatalog))
                {
                    initialCatalog = value;
                    OnPropertyChanged("InitialCatalog");
                }
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private string userId;
        public string UserId
        {
            get { return userId; }
            set
            {
                if (!string.IsNullOrEmpty(value)
                    && !value.Equals(userId))
                {
                    userId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// ********************************************************************/

        /// <summary>
        /// 连接数据服务端的参数
        /// </summary>
        /// ********************************************************************/
        /// <summary>
        /// 数据中心的Ip地址
        /// </summary>
        private string serverIp;
        public string ServerIp
        {
            get { return serverIp; }
            set
            {
                if (!string.IsNullOrEmpty(value)
                    && !value.Equals(serverIp))
                {
                    serverIp = value;
                    OnPropertyChanged("ServerIp");
                }
            }
        }

        /// <summary>
        /// 连接到数据中心服务端的端口号
        /// </summary>
        private string serverPort;
        public string ServerPort
        {
            get { return serverPort; }
            set
            {
                if (!string.IsNullOrEmpty(value)
                    && !value.Equals(serverPort))
                {
                    serverPort = value;
                    OnPropertyChanged("ServerPort");
                }
            }
        }
        /// ********************************************************************/


        /// <summary>
        /// 外径上限值
        /// </summary>
        private double outerDiameterUpper;
        public double OuterDiameterUpper
        {
            get { return outerDiameterUpper; }
            set
            {
                if (value != outerDiameterUpper)
                {
                    outerDiameterUpper = value;
                    OnPropertyChanged("OuterDiameterUpper");
                }
            }
        }

        /// <summary>
        /// 外径下限值
        /// </summary>
        private double outerDiameterLower;
        public double OuterDiameterLower
        {
            get { return outerDiameterLower; }
            set
            {
                if (value != outerDiameterLower)
                {
                    outerDiameterLower = value;
                    OnPropertyChanged("OuterDiameterLower");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
