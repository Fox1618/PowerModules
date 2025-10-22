using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Tonhe
{
    public class SPackTonheTiming : SPackTonheBase
    {
        public SPackTonheTiming()
        {
            CommandType = (byte) CommandCodesTonhe.TimingCmd;
            ModuleAddress = byte.MaxValue;
        }
        public override byte[] Data => new byte[8];
    }
}
