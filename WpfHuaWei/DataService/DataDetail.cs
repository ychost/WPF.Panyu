using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WpfHuaWei.DataService
{
    [DataContract]
    public class DataDetail
    {
        [DataMember(Order = 0)]
        public int ParameterCodeID { get; set; }

        [DataMember(Order = 1)]
        public double CollectedValue { get; set; }

        [DataMember(Order = 2)]
        public string CollectedTime { get; set; }

        [DataMember(Order = 3)]
        public string Axis_No { get; set; }

        [DataMember(Order = 4)]
        public int MachineID { get; set; }
    }
}
