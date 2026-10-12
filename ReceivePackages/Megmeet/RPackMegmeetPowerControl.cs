namespace Telemetria.Hardware.PowerModules.ReceivePackages.Megmeet
{
    public class RPackMegmeetPowerControl : RPackMegmeetBase
    {
        public RPackMegmeetPowerControl(byte[] data) : base(data)
        {
            IsStart = Data[7] == 0xAA;
        }

        public bool IsStart { get; set; }
    }
}
