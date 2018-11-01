using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HuaWeiBase
{
    public class OutlierInfo : INotifyPropertyChanged
    {
        /// <summary>
        /// 序号
        /// </summary>
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 半成品卡号
        /// </summary>
        private string axisNo;
        public string AxisNo
        {
            get { return axisNo; }
            set
            {
                axisNo = value;
                OnPropertyChanged("AxisNo");
            }
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        private string parameterName;
        public string ParameterName
        {
            get { return parameterName; }
            set
            {
                parameterName = value;
                OnPropertyChanged("ParameterName");
            }
        }

        /// <summary>
        /// 采集值
        /// </summary>
        private double value;
        public double Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// 米数
        /// </summary>
        private double meter;
        public double Meter
        {
            get { return meter; }
            set
            {
                meter = value;
                OnPropertyChanged("Meter");
            }
        }

        /// <summary>
        /// 采集时间
        /// </summary>
        private DateTime collectedTime;
        public DateTime CollectedTime
        {
            get { return collectedTime; }
            set
            {
                collectedTime = value;
                OnPropertyChanged("CollectedTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
