using HuaWeiUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WpfHuaWei.DataService;
using WpfHuaWei.DeviceView;

namespace WpfHuaWei.Utils
{
    public class ApplicationStateManager
    {
        private const string TempPath = "Temp\\";

        private const string WorksheetNoPath = "Temp\\wsn.bin";

        private const string ProductionInfoPath = "Temp\\mpi.bin";

        public static bool SaveMachineState(BaseMachine machine)
        {
            bool bResult = true;
            bResult &= SerializeUtil.
                SerializableObject(WorksheetNoPath, machine.WorksheetNo);
            bResult &= SerializeUtil.
                SerializableObject(ProductionInfoPath, machine.ProductionInfo);
            return bResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machine"></param>
        public static void RecoveryMachineState(BaseMachine machine)
        {
            machine.WorksheetNo =
                SerializeUtil.DeserializableObject<string>(WorksheetNoPath);

            machine.ProductionInfo = SerializeUtil.
                DeserializableObject<MachineProductionInfo>(ProductionInfoPath);
            if (machine.ProductionInfo != null)
                machine.ProductionInfo.InitializeProductionInfo();
        }

        /// <summary>
        /// 清空存储应用程序状态的目录
        /// </summary>
        public static void ClearApplicationStatePath()
        {
            try
            {
                Directory.Delete(TempPath, true);
                Directory.CreateDirectory(TempPath);
            }
            catch { }
        }
    }
}
