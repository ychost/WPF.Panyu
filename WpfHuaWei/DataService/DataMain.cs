using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WpfHuaWei.DataService
{
    [DataContract]
    public class DataMain
    {
        [DataMember(Order = 0)]
        public string TaskID { get; set; }

        [DataMember(Order = 1)]
        public string SpecificationID { get; set; }

        [DataMember(Order = 2)]
        public int MachineID { get; set; }

        [DataMember(Order = 3)]
        public int MachineTypeID { get; set; }

        [DataMember(Order = 4)]
        public string EmployeeID_Main { get; set; }

        [DataMember(Order = 5)]
        public string EmployeeID_Assistant { get; set; }

        [DataMember(Order = 6)]
        public string Start_Axis_No { get; set; }

        /// <summary>
        /// 真实卡
        /// </summary>
        [DataMember(Order = 7)]
        public string CodeNumber { get; set; }

        /// <summary>
        /// 生成卡
        /// </summary>
        [DataMember(Order = 8)]
        public string Axis_No { get; set; }

        [DataMember(Order = 9)]
        public string Printcode { get; set; }

        [DataMember(Order = 10)]
        public string CollectedTime { get; set; }

        [DataMember(Order = 11)]
        public string MaterialRFID { get; set; }
    }
}
