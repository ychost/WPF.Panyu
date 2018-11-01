using HuaWeiBase;
using HuaWeiDAL;
using HuaWeiModel;
using HuaWeiUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace WpfHuaWei.DataService {
    [Serializable]
    public class MachineProductionInfo : INotifyPropertyChanged {
        /// <summary>
        /// dataDetailList的写锁
        /// </summary>
        [NonSerialized]
        private object dataDetailLock;

        /// <summary>
        /// 一个简单的生产者消费者模式中的商品集合
        /// 缓存DataDetail数据，分离数据中心的存储和"界面的读取"
        /// </summary>
        [NonSerialized]
        private List<DataDetail> dataDetailList;

        /// <summary>
        /// 工艺规格文件服务
        /// </summary>
        public ProductionProcessPdfService PdfService { get; set; }

        public MachineProductionInfo() {
            InitializeProductionInfo();
        }

        public void InitializeProductionInfo() {
            dataDetailLock = new object();
            dataDetailList = new List<DataDetail>();

            PdfService = new ProductionProcessPdfService();

            ThreadPool.QueueUserWorkItem(ReadAndUpdateProductionInfo, null);
        }

        /// <summary>
        /// 机台名称
        /// </summary>
        private string machineName;
        public string MachineName {
            get { return machineName; }
            set {
                machineName = value;
                OnPropertyChanged("MachineName");
            }
        }

        /// <summary>
        /// 操作卡号
        /// </summary>
        private string employeeRfid;
        public string EmployeeRfid {
            set {
                if (string.IsNullOrEmpty(value) ||
                    value.Equals(employeeRfid)) return;

                employeeRfid = value;
                if (Configuration.EmployeeMap.ContainsKey(value))
                    EmployeeName = Configuration.EmployeeMap[value].EmployeeName;
                else {
                    string whereString = "where employeeCode='" + value + "'";
                    Employee employee = EmployeeDAL.GetEmployee(whereString);
                    if (employee != null) {
                        EmployeeName = employee.EmployeeName;
                        Configuration.EmployeeMap.Add(employee.EmployeeCode, employee);
                    }
                }
            }
        }

        /// <summary>
        /// 操作员姓名
        /// </summary>
        private string employeeName;
        public string EmployeeName {
            get { return employeeName; }
            set {
                employeeName = value;
                OnPropertyChanged("EmployeeName");
            }
        }

        /// <summary>
        /// 半成品卡号
        /// </summary>
        private string axisNo;
        public string AxisNo {
            get { return axisNo; }
            set {
                if (!string.IsNullOrEmpty(value)
                    && !value.Equals(axisNo)) {
                    axisNo = value;
                    OnPropertyChanged("AxisNo");
                }
            }
        }

        /// <summary>
        /// 米数
        /// </summary>
        private double meter;
        public double Meter {
            get { return meter; }
            set {
                if (value != meter) {
                    meter = value;
                    OnPropertyChanged("Meter");
                }
            }
        }

        /// <summary>
        /// 外径值
        /// </summary>
        private double outerDiameter;
        public double OuterDiameter {
            set {
                if (outerDiameter != value) {
                    if (value > maxOD)
                        MaxOD = value;
                    if (value < minOD || minOD <= 0)
                        MinOD = value;

                    if (value <= 0) {
                        Thickness = 0;
                        MinThickness = 0;
                    }
                    // 如果有铜线外径
                    else if (innerDiameter > 0) {
                        Thickness = (value - innerDiameter) / 2.0;
                        if (thickness < minThickness || minThickness <= 0)
                            MinThickness = thickness;
                    }

                    outerDiameter = value;
                }
            }
        }

        /// <summary>
        /// 导体直径
        /// </summary>
        private double innerDiameter;
        public double InnerDiameter {
            set {
                if (innerDiameter != value) {
                    if (value <= 0) {
                        Thickness = 0;
                        MinThickness = 0;
                    }
                    innerDiameter = value;
                }
            }
        }

        /// <summary>
        /// 最大OD值
        /// </summary>
        private double maxOD;
        public double MaxOD {
            get { return maxOD; }
            set {
                maxOD = value;
                OnPropertyChanged("MaxOD");
            }
        }

        /// <summary>
        /// 最小OD值
        /// </summary>
        private double minOD;
        public double MinOD {
            get { return minOD; }
            set {
                minOD = value;
                OnPropertyChanged("MinOD");
            }
        }

        /// <summary>
        /// 绝缘/护套厚度
        /// </summary>
        private double thickness = int.MinValue;
        public double Thickness {
            get { return thickness; }
            set {
                if (thickness != value) {
                    if (value > 0) {
                        thickness = value;
                        OnPropertyChanged("Thickness");
                    } else if (thickness > 0) {
                        thickness = 0;
                        OnPropertyChanged("Thickness");
                    }
                }
            }
        }

        /// <summary>
        /// 最小绝缘/护套厚度
        /// </summary>
        private double minThickness = int.MinValue;
        public double MinThickness {
            get { return minThickness; }
            set {
                if (minThickness != value) {
                    if (value > 0) {
                        minThickness = value;
                        OnPropertyChanged("MinThickness");
                    } else if (minThickness > 0) {
                        minThickness = 0;
                        OnPropertyChanged("MinThickness");
                    }
                }
            }
        }

        /// <summary>
        /// 物料射频卡号
        /// </summary>
        private string materialRfid;
        public string MaterialRfid {
            set {
                if (!string.IsNullOrEmpty(value)
                    && !value.Equals(materialRfid)) {
                    string where = "where MaterialRFID = '" + value + "'";
                    Material material = MaterialDAL.GetMaterial(where);
                    if (material != null) {
                        materialRfid = value;
                        MaterialName = material.MaterialName;
                        MaterialColor = material.MaterialColor;
                    }
                }
            }
        }

        /// <summary>
        /// 材料名称
        /// </summary>
        private string materialName;
        public string MaterialName {
            get { return materialName; }
            set {
                materialName = value;
                OnPropertyChanged("MaterialName");
            }
        }

        /// <summary>
        /// 材料颜色
        /// </summary>
        private string materialColor;
        public string MaterialColor {
            get { return materialColor; }
            set {
                materialColor = value;
                OnPropertyChanged("MaterialColor");
            }
        }

        /// <summary>
        /// 报警次数
        /// </summary>
        private int sparkValue;
        public int SparkValue {
            get { return sparkValue; }
            set {
                if (sparkValue != value) {
                    sparkValue = value;
                    OnPropertyChanged("SparkValue");
                }
            }
        }

        /// <summary>
        /// 主机速度
        /// </summary>
        private double motorVelocity = int.MinValue;
        public double MotorVelocity {
            get { return motorVelocity; }
            set {
                if (motorVelocity != value) {
                    motorVelocity = value;
                    OnPropertyChanged("MotorVelocity");
                }
            }
        }

        /// <summary>
        /// 线速度
        /// </summary>
        private double lineVelocity = int.MinValue;
        public double LineVelocity {
            get { return lineVelocity; }
            set {
                if (value != lineVelocity) {
                    lineVelocity = value;
                    OnPropertyChanged("LineVelocity");
                }
            }
        }

        /// <summary>
        /// 放线张力
        /// </summary>
        private double firststress = int.MinValue;
        public double Firststress {
            get { return firststress; }
            set {
                if (firststress != value) {
                    firststress = value;
                    OnPropertyChanged("Firststress");
                }
            }
        }

        /// <summary>
        /// 一段温度
        /// </summary>
        private double temperature1 = int.MinValue;
        public double Temperature1 {
            get { return temperature1; }
            set {
                if (temperature1 != value) {
                    temperature1 = value;
                    OnPropertyChanged("Temperature1");
                }
            }
        }

        /// <summary>
        /// 二段温度
        /// </summary>
        private double temperature2 = int.MinValue;
        public double Temperature2 {
            get { return temperature2; }
            set {
                if (temperature2 != value) {
                    temperature2 = value;
                    OnPropertyChanged("Temperature2");
                }
            }
        }

        /// <summary>
        /// 三段温度
        /// </summary>
        private double temperature3 = int.MinValue;
        public double Temperature3 {
            get { return temperature3; }
            set {
                if (temperature3 != value) {
                    temperature3 = value;
                    OnPropertyChanged("Temperature3");
                }
            }
        }

        /// <summary>
        /// 四段温度
        /// </summary>
        private double temperature4 = int.MinValue;
        public double Temperature4 {
            get { return temperature4; }
            set {
                if (temperature4 != value) {
                    temperature4 = value;
                    OnPropertyChanged("Temperature4");
                }
            }
        }

        /// <summary>
        /// 五段温度
        /// </summary>
        private double temperature5 = int.MinValue;
        public double Temperature5 {
            get { return temperature5; }
            set {
                if (temperature5 != value) {
                    temperature5 = value;
                    OnPropertyChanged("Temperature5");
                }
            }
        }

        /// <summary>
        /// 六段温度
        /// </summary>
        private double temperature6 = int.MinValue;
        public double Temperature6 {
            get { return temperature6; }
            set {
                if (temperature6 != value) {
                    temperature6 = value;
                    OnPropertyChanged("Temperature6");
                }
            }
        }

        /// <summary>
        /// 颈部温度
        /// </summary>
        private double temperature7 = int.MinValue;
        public double Temperature7 {
            get { return temperature7; }
            set {
                if (temperature7 != value) {
                    temperature7 = value;
                    OnPropertyChanged("Temperature7");
                }
            }
        }

        /// <summary>
        /// 机头温度
        /// </summary>
        private double temperature8 = int.MinValue;
        public double Temperature8 {
            get { return temperature8; }
            set {
                if (temperature8 != value) {
                    temperature8 = value;
                    OnPropertyChanged("Temperature8");
                }
            }
        }

        /// <summary>
        /// 眼膜温度
        /// </summary>
        private double temperature9 = int.MinValue;
        public double Temperature9 {
            get { return temperature9; }
            set {
                if (temperature9 != value) {
                    temperature9 = value;
                    OnPropertyChanged("Temperature9");
                }
            }
        }

        /// <summary>
        /// 火花机电压
        /// </summary>
        private double sparkVoltage;

        public double SparkVoltage {
            get { return sparkVoltage; }
            set {
                if (sparkVoltage != null) {
                    sparkVoltage = value;
                    OnPropertyChanged(nameof(SparkVoltage));
                }
            }
        }


        /// <summary>
        /// 水槽温度
        /// </summary>
        private double temperature10 = int.MinValue;
        public double Temperature10 {
            get { return temperature10; }
            set {
                if (temperature10 != value) {
                    temperature10 = value;
                    OnPropertyChanged("Temperature10");
                }
            }
        }

        /// <summary>
        /// 现场温度
        /// </summary>
        private double temperature = int.MinValue;
        public double Temperature {
            get { return temperature; }
            set {
                if (temperature != value) {
                    temperature = value;
                    OnPropertyChanged("Temperature");
                }
            }
        }

        /// <summary>
        /// 现场湿度
        /// </summary>
        private double humidity = int.MinValue;
        public double Humidity {
            get { return humidity; }
            set {
                if (humidity != value) {
                    humidity = value;
                    OnPropertyChanged("Humidity");
                }
            }
        }

        #region 1-9段温度，水槽温度，线速度，放线张力，外径值，铜线OD
        // 1     人员卡
        // 2     0d值
        // 3     现场温度
        // 4     现场湿度
        // 5     后端计米
        // 7     喷码机盘号
        // 8     喷码机米数
        // 9     放线张力
        // 10    上到工序卡
        // 11    生产半成品卡
        // 12    原材料卡
        // 13    一段温度显示值
        // 14    一段温度实测值
        // 15    二段温度显示值
        // 16    二段温度实测值
        // 17    三段温度显示值
        // 18    三段温度实测值
        // 19    四段温度显示值
        // 20    四段温度实测值
        // 21    五段温度显示值
        // 22    五段温度实测值
        // 23    六段温度显示值
        // 24    六段温度实测值
        // 25    颈部温度显示值
        // 26    颈部温度实测值
        // 27    机头温度显示值
        // 28    机头温度实测值
        // 29    眼模温度显示值
        // 30    眼模温度实测值
        // 31    水槽温度显示值
        // 32    水槽温度实测值
        // 33    计米设定值
        // 34    计米实测值
        // 35    主机电机转速
        // 36    主机螺杆转速
        // 37    引取电机转速
        // 38    线速度
        // 39    凹凸检出
        // 40    火花检出
        // 41    故障
        // 42    铜线OD
        // 43    喷码时间
        // 60    落轴
        // 800 火花机电压
        /// <summary>
        /// 定义double属性变化的委托处理对象
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="value"></param>
        public delegate void DoublePropertyChangedHandler(int parameterId,
            DateTime dateTime, double value);

        public event DoublePropertyChangedHandler DoublePropertyChanged;

        private void OnDoublePropertyChanged(int parameterId, DateTime dateTime, double value) {
            if (DoublePropertyChanged != null) {
                DoublePropertyChanged.Invoke(parameterId, dateTime, value);
            }
        }

        /// <summary>
        /// 添加DataDetail
        /// </summary>
        /// <param name="dataDetail"></param>
        public void AddDataDetail(DataDetail dataDetail) {
            lock (dataDetailLock) {
                dataDetailList.Add(dataDetail);
            }
        }

        /// <summary>
        /// 添加DataDetail集合
        /// </summary>
        /// <param name="dataDetails"></param>
        public void AddDataDetailList(IEnumerable<DataDetail> dataDetails) {
            lock (dataDetailLock) {
                foreach (var dd in dataDetails) {
                    dataDetailList.Add(dd);
                }
            }
        }

        /// <summary>
        /// 读取并更新生产数据
        /// </summary>
        /// <param name="state"></param>
        private void ReadAndUpdateProductionInfo(object state) {
            while (true) {
                if (dataDetailList.Count > 0) {
                    DataDetail dataDetail = null;
                    lock (dataDetailLock) {
                        dataDetail = dataDetailList[0];
                        dataDetailList.RemoveAt(0);
                    }
                    if (dataDetail != null) {
                        AddSampleValue(dataDetail);
                    }
                }
                Thread.Sleep(5); // 线程稍微睡眠，防止CPU占用过高
            }
        }

        private DateTime lastOuterDiameterRecvTime = DateTime.Now;

        /// <summary>
        /// 添加采集数据
        /// </summary>
        private void AddSampleValue(DataDetail dataDetail) {
            switch (dataDetail.ParameterCodeID) {
                case 2: // od值
                    if ((DateTime.Now - lastOuterDiameterRecvTime).TotalMilliseconds > 300) {
                        OuterDiameter = dataDetail.CollectedValue;
                        OnDoublePropertyChanged(2, ParseDateTime(dataDetail.CollectedTime),
                            dataDetail.CollectedValue);

                        lastOuterDiameterRecvTime = DateTime.Now;
                    }
                    break;
                case 3: // 现场温度
                    Temperature = dataDetail.CollectedValue;
                    break;
                case 4: // 现场湿度
                    Humidity = dataDetail.CollectedValue;
                    break;
                case 34: // 计米实测值(PLC上的米数)
                    Meter = dataDetail.CollectedValue;
                    break;
                case 9: // 放线张力
                    Firststress = dataDetail.CollectedValue;
                    break;
                case 13: // 一段温度显示值
                    Temperature1 = dataDetail.CollectedValue;
                    break;
                case 15: // 二段温度显示值
                    Temperature2 = dataDetail.CollectedValue;
                    break;
                case 17: // 三段温度显示值
                    Temperature3 = dataDetail.CollectedValue;
                    break;
                case 19: // 四段温度显示值
                    Temperature4 = dataDetail.CollectedValue;
                    break;
                case 21: // 五段温度显示值
                    Temperature5 = dataDetail.CollectedValue;
                    break;
                case 23: // 六段温度显示值
                    Temperature6 = dataDetail.CollectedValue;
                    break;
                case 25: // 颈部温度显示值
                    Temperature7 = dataDetail.CollectedValue;
                    break;
                case 27: // 机头温度显示值
                    Temperature8 = dataDetail.CollectedValue;
                    break;
                case 29: // 眼模温度显示值
                    Temperature9 = dataDetail.CollectedValue;
                    break;
                case 31: // 水槽温度显示值
                    Temperature10 = dataDetail.CollectedValue;
                    break;
                case 35: // 主机电机转速
                    if (dataDetail.CollectedValue >= 0 &&
                        dataDetail.CollectedValue < GlobalConstants.GlobalUpper) {
                        MotorVelocity = dataDetail.CollectedValue;
                    }
                    break;
                case 38: // 线速度
                    if (dataDetail.CollectedValue >= 0 &&
                        dataDetail.CollectedValue < GlobalConstants.GlobalUpper) {
                        LineVelocity = dataDetail.CollectedValue;
                    }
                    break;
                case 40: // 火花检出
                    SparkValue = (int)dataDetail.CollectedValue;
                    break;
                case 42: // 铜线OD（导体直径）
                    InnerDiameter = dataDetail.CollectedValue;
                    OnDoublePropertyChanged(42, ParseDateTime(dataDetail.CollectedTime),
                        dataDetail.CollectedValue);
                    break;
                case 800:
                    SparkVoltage = dataDetail.CollectedValue;
                    break;
            }
        }

        /// <summary>
        /// 从字符串中解析DateTime对象
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns></returns>
        private DateTime ParseDateTime(string dateTimeString) {
            DateTime dateTime = DateTime.Now;
            DateTime.TryParse(dateTimeString, out dateTime);
            return dateTime;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            if (this.PropertyChanged != null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
