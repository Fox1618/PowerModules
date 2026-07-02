namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetReadVoltage : SPackMegmeetBase
    {
        public SPackMegmeetReadVoltage(byte moduleAddress) : base(moduleAddress, MegmeetCommands.VoltageRead)
        {
        }
    }
}
