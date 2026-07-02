namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetSetVoltage : SPackMegmeetBase
    {
        public SPackMegmeetSetVoltage(byte moduleAddress, float voltage) : base(moduleAddress, MegmeetCommands.VoltageSet)
        {
            Voltage = voltage;
            SetFloatValue(voltage);
        }

        public float Voltage { get; }
    }
}
