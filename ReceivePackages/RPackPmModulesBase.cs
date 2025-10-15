using Hardware.DeviceSending;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.ReceivePackages
{
    public class RPackPmModulesBase : RPackBase, IRPackPmModulesBase
    {
        public RPackPmModulesBase(byte[] data) : base(data)
        {

        }
        public byte MonitorAddress { get; set; }
        public byte ModuleAddress { get; set; }
        public byte[] Data { get; protected set; }

    }
}
