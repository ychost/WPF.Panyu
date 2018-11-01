using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiBase
{
    public class CableType
    {
        /// <summary>
        /// 半成品线材名称
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 线材生产所在流程的名称
        /// </summary>
        private string processName;
        public string ProcessName
        {
            get { return processName; }
            set
            {
                processName = value;
                OnPropertyChanged("ProcessName");
            }
        }

        /// <summary>
        /// 线材生产的各项参数标准
        /// </summary>
        private List<DataPattern> patterns = new List<DataPattern>();
        public List<DataPattern> Patterns
        {
            get { return patterns; }
            set { patterns = value; }
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
