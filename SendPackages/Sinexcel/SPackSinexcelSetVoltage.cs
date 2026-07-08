using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Sinexcel
{
    public sealed class SPackSinexcelSetVoltage : SPackSinexcelBase
    {
        public SPackSinexcelSetVoltage(byte group, byte subAddress, float voltage) : base(group, subAddress, (ushort)CommandCodesSinexcel.SetOutputVoltage)
        {
            Voltage = voltage;
            SetVoltageValue(voltage);
        }

        public float Voltage { get; }
    }
}
