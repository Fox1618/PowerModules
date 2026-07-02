namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetReadStatus : SPackMegmeetBase
    {
        public SPackMegmeetReadStatus(byte moduleAddress) : base(moduleAddress, MegmeetCommands.StatusRead)
        {
        }
    }
}
