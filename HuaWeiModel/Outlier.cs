using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuaWeiModel
{
    public class Outlier
    {
        public string MachineName { get; set; }

        public string ParamName { get; set; }

        public double Value { get; set; }

        public double Upper { get; set; }

        public double Lower { get; set; }

        public DateTime CollectedTime { get; set; }
    }
}
