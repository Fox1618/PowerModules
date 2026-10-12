using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetSetVoltage : SPackMegmeetBase
    {
        public SPackMegmeetSetVoltage(byte moduleAddress, float voltage) : base(moduleAddress, (ushort)CommandCodesMegmeet.VoltageSet)
        {
            Voltage = voltage;
            SetFloatValue(voltage);
        }

        public float Voltage { get; }
    }
}
