namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetReadCurrent : SPackMegmeetBase
    {
        public SPackMegmeetReadCurrent(byte moduleAddress) : base(moduleAddress, MegmeetCommands.CurrentRead)
        {
        }
    }
}
