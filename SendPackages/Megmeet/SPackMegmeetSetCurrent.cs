using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetSetCurrent : SPackMegmeetBase
    {
        public SPackMegmeetSetCurrent(byte moduleAddress, float current) : base(moduleAddress, (ushort)CommandCodesMegmeet.CurrentSet)
        {
            Current = current;
            // Value is divided by 1000 to send it to the device.
            // So we need to correct it by multiplying by COEF_AV
            float CorrectedCurrent = Current * COEF_AV;
            SetFloatValue(CorrectedCurrent);
        }

        public float Current { get; }
    }
}
