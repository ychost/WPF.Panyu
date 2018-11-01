using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppResources
{
    internal class StringDictionary
    {
        /// <summary>
        /// 用一个字典保存应用程序用到的资源
        /// 它的作用于Application.Current.Properties是一样的，
        /// 只不过专门独立出主应用程序之外，放到dll中来
        /// </summary>
        public readonly static Dictionary<string, string> StringRes = new Dictionary<string, string>();

        static StringDictionary()
        {
            StringRes.Add("Auto_Id", "序号");
            StringRes.Add("Company_ID", "公司ID");
            StringRes.Add("Contrast_ID", "合同ID");
            StringRes.Add("Operator_RFID", "操作员");
            StringRes.Add("Machine_NO", "机台编号");
            StringRes.Add("Material_RFID", "胶料");
            StringRes.Add("Temperature1", "一段温度");
            StringRes.Add("Temperature2", "二段温度");
            StringRes.Add("Temperature3", "三段温度");
            StringRes.Add("Temperature4", "四段温度");
            StringRes.Add("Temperature5", "五段温度");
            StringRes.Add("Temperature6", "六段温度");
            StringRes.Add("Temperature7", "颈部温度");
            StringRes.Add("Temperature8", "机头温度");
            StringRes.Add("Temperature9", "眼膜温度");
            StringRes.Add("Temperature10", "水槽温度");
            StringRes.Add("OuterDiameter", "外径值");
            StringRes.Add("TemperatureSet1", "一段温度设定值");
            StringRes.Add("TemperatureSet2", "二段温度设定值");
            StringRes.Add("TemperatureSet3", "三段温度设定值");
            StringRes.Add("TemperatureSet4", "四段温度设定值");
            StringRes.Add("TemperatureSet5", "五段温度设定值");
            StringRes.Add("TemperatureSet6", "六段温度设定值");
            StringRes.Add("TemperatureSet7", "颈部温度设定值");
            StringRes.Add("TemperatureSet8", "机头温度设定值");
            StringRes.Add("TemperatureSet9", "眼膜温度设定值");
            StringRes.Add("TemperatureSet10", "水槽温度设定值");
            StringRes.Add("OuterDiameter", "外径值");
            StringRes.Add("Outpress", "挤出压力");
            StringRes.Add("Firststress", "放线张力");
            StringRes.Add("SavelineStress", "储线张力");
            StringRes.Add("FinishedStress", "收线张力");
            StringRes.Add("FirstVelocity", "放线速度");
            StringRes.Add("MotorVelocity", "主机速度");
            StringRes.Add("FinishedVelocity", "收线速度");
            StringRes.Add("Concave_convex_value", "凹凸值");
            StringRes.Add("Concave_convex_set_value", "凹凸设定值");
            StringRes.Add("Spark_value", "火花值");
            StringRes.Add("Spark_set_value", "火花设定值");
            StringRes.Add("Last_meter", "后端计米");
            StringRes.Add("First_meter", "前段计米");
            StringRes.Add("Collected_DATE", "采集时间");
            StringRes.Add("Volume_No", "盘号");
            StringRes.Add("Pro_Volume_No", "来料盘号");
            StringRes.Add("Meter_Tag", "米标");
            StringRes.Add("Retained_field1", "");
            StringRes.Add("Retained_field2", "主机螺杆转速");
            StringRes.Add("Retained_field3", "");
            StringRes.Add("Retained_field4", "");
            StringRes.Add("Retained_field5", "");
            StringRes.Add("Retained_field6", "");
            StringRes.Add("Retained_field7", "铜线外径");
            StringRes.Add("Retained_field8", "印制盘号");
            StringRes.Add("Retained_field9", "印制米数");
            StringRes.Add("Retained_field11", "湿度1");
            StringRes.Add("Retained_field12", "温度1");
            StringRes.Add("Retained_field13", "湿度2");
            StringRes.Add("Retained_field14", "温度2");

            // 机台名称映射
            StringRes.Add("绝缘押出机603#", "603机台");
            StringRes.Add("绝缘押出机604#", "604机台");
            StringRes.Add("绝缘押出机605#", "605机台");
            StringRes.Add("绝缘押出机606#", "606机台");
            StringRes.Add("护套押出机903#", "903机台");
            StringRes.Add("护套押出机904#", "904机台");
            StringRes.Add("护套押出机906#", "906机台");

            StringRes.Add("603机台", "绝缘押出机603#");
            StringRes.Add("604机台", "绝缘押出机604#");
            StringRes.Add("605机台", "绝缘押出机605#");
            StringRes.Add("606机台", "绝缘押出机606#");
            StringRes.Add("903机台", "护套押出机903#");
            StringRes.Add("904机台", "护套押出机904#");
            StringRes.Add("906机台", "护套押出机906#");
        }
    }
}
