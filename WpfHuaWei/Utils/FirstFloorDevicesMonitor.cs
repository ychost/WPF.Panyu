using HuaWeiBase;
using HuaWeiUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfHuaWei.Utils
{
    public class FirstFloorDevicesMonitor
    {
        /// <summary>
        /// 底层设备列表
        /// </summary>
        public static List<FirstFloorDevice> Devices = null;
        /// <summary>
        /// 用于同步对Devices对象的访问
        /// </summary>
        private static object MsgSync = new object();
        /// <summary>
        /// 是否正在监测底层设备上线状态
        /// </summary>
        private static bool IsMonitoring = false;

        static FirstFloorDevicesMonitor()
        {
            Devices = XmlUtil.GetModelFromXmlFile<FirstFloorDevice>(XmlUtil.FirstFloorDevice);
        }

        /// <summary>
        /// 获取底层设备的上线状态
        /// </summary>
        /// <param name="deviceIPAddr"></param>
        /// <returns></returns>
        public static bool? IsDeviceOnline(string deviceIPAddr)
        {
            bool? state = null;
            if(Monitor.TryEnter(MsgSync, 30))
            {
                if(Devices != null && Devices.Count > 0)
                {
                    foreach(var device in Devices)
                    {
                        if(device.DeviceIPAddr.Equals(deviceIPAddr))
                        {
                            state = device.IsOnline;
                            break;
                        }
                        else { continue; }
                    }
                }
            }
            return state;
        }

        /// <summary>
        /// 开启一个工作线程监测底层设备
        /// </summary>
        public static void StartMonitoringDevices()
        {
            if(!IsMonitoring && Devices != null && Devices.Count > 0)
            {
                IsMonitoring = true;
                ThreadPool.QueueUserWorkItem(MonitoringDevicesWorkItem, null);
            }
        }

        /// <summary>
        /// 监测底层设备状态的工作线程函数
        /// </summary>
        /// <param name="state"></param>
        private static void MonitoringDevicesWorkItem(object state)
        {
            int deviceIndex = 0;
            int deviceCount = 0;
            string deviceIpAddr = string.Empty;
            while(IsMonitoring)
            {
                if(Devices != null && Monitor.TryEnter(MsgSync, 30))
                {
                    deviceCount = Devices.Count;
                    if(deviceIndex >= deviceCount)
                    {
                        deviceIndex = 0;
                    }
                    deviceIpAddr = Devices[deviceIndex].DeviceIPAddr;
                }
                else
                {
                    Thread.Sleep(100);
                    continue;
                }

                Ping pingSender = new Ping();
                PingReply reply = pingSender.Send(deviceIpAddr, 200);

                bool isOnline = reply.Status == IPStatus.Success;
                if(Monitor.TryEnter(MsgSync, 30))
                {
                    Devices[deviceIndex].IsOnline = isOnline;
                }

                deviceIndex += 1;
                Thread.Sleep(100);
            }
        }

        public static void StopMonitoringDevices()
        {
            IsMonitoring = false;
        }
    }
}
