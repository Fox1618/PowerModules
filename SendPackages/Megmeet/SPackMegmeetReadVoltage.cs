using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetReadVoltage : SPackMegmeetBase
    {
        public SPackMegmeetReadVoltage(byte moduleAddress) : base(moduleAddress, (ushort)CommandCodesMegmeet.VoltageRead)
        {
        }
    }
}
