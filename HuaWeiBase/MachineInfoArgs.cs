using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HuaWeiBase
{
    public class MachineInfoArgs : RoutedEventArgs
    {
        /// <summary>
        /// 机台信息
        /// </summary>
        public MachineInfo MachineInfo { get; set; }

        public MachineInfoArgs() : base() { }

        public MachineInfoArgs(RoutedEvent routedEvent)
            : base(routedEvent) { }

        public MachineInfoArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source) { }
    }
}
