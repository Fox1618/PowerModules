using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetReadStatus : SPackMegmeetBase
    {
        public SPackMegmeetReadStatus(byte moduleAddress) : base(moduleAddress, (ushort)CommandCodesMegmeet.StatusRead)
        {
        }
    }
}
