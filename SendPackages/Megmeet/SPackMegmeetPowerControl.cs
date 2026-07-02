namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetPowerControl : SPackMegmeetBase
    {
        public SPackMegmeetPowerControl(byte moduleAddress, bool isStart) : base(moduleAddress, MegmeetCommands.PowerControl)
        {
            IsStart = isStart;
            Data[7] = isStart ? (byte)0xAA : (byte)0x55;
        }

        public bool IsStart { get; }
    }
}
