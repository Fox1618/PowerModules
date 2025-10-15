using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe;

namespace Telemetria.Hardware.PowerModules.SendPackages.Tonhe
{
    public class SPackTonheTiming : SPackTonheBase
    {
        public SPackTonheTiming()
        {
            CommandType = (byte) CommandCodesTonhe.TimingCmd;
            MonitorAddress = byte.MaxValue;
        }
        public override byte[] Data => new byte[8];
    }
}
