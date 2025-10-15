using Hardware.DeviceSending;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages
{
    public class SPackPModulesBase : SPackBase, ISPackPmModulesBase
    {
        public byte MonitorAddress { get; set; }
        public byte ModuleAddress { get; set; }
        public virtual byte[] Data { get; set; }
        public byte CommandType { get; set; }


        public override byte[] ToByte()
        {
            return null;
        }
    }
}
