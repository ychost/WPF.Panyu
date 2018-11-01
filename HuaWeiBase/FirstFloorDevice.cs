using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiBase
{
    /// <summary>
    /// 底层模块状态信息
    /// </summary>
    public class FirstFloorDevice : INotifyPropertyChanged
    {
        /// <summary>
        /// 模块所在的流程名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 模块所在的机台名称
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// 模块的名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 模块的IP地址
        /// </summary>
        public string DeviceIPAddr { get; set; }

        /// <summary>
        /// 模块是否在线
        /// </summary>
        private bool isOnline = false;
        public bool IsOnline
        {
            get { return isOnline; }
            set
            {
                if(isOnline != value)
                {
                    isOnline = value;
                    OnPropertyChanged("IsOnline");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
