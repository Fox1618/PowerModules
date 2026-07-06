using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetSetCurrent : SPackMegmeetBase
    {
        public SPackMegmeetSetCurrent(byte moduleAddress, float current) : base(moduleAddress, (ushort)CommandCodesMegmeet.CurrentSet)
        {
            Current = current;
            SetFloatValue(current);
        }

        public float Current { get; }
    }
}
