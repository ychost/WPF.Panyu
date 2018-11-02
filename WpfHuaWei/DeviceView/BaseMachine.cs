using HuaWeiBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HuaWeiDAL;
using HuaWeiModel;
using WpfHuaWei.DataService;
using WpfHuaWei.Utils;

namespace WpfHuaWei.DeviceView {
    /// <summary>
    /// BaseDevice.xaml 的交互逻辑
    /// </summary>
    public class BaseMachine : UserControl {
        public string MachineName { get; set; }

        protected string simpleWorkSheetNo = "SJ037-2017040601A";

        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorksheetNo { get; set; }

        /// <summary>
        /// 当前机台的生产数据信息
        /// </summary>
        public MachineProductionInfo ProductionInfo { get; set; }

        public BaseMachine() { }

        public virtual void StartLoadingData() { }

        public virtual void StartLoadingData(string machineName) {
            MachineName = machineName;
        }

        /// <summary>
        /// 判断是否是工单号
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        protected bool isWorksheetNo(string ws) {
            if (string.IsNullOrEmpty(ws) || ws.Length != simpleWorkSheetNo.Length) {
                return false;
            }
            return true;
        }

    }
}