using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetReadCurrent : SPackMegmeetBase
    {
        public SPackMegmeetReadCurrent(byte moduleAddress) : base(moduleAddress, (ushort)CommandCodesMegmeet.CurrentRead)
        {
        }
    }
}
