namespace Telemetria.Hardware.PowerModules.ReceivePackages.Sinexcel
{
    public class RPackSinexcelPowerControl : RPackSinexcelBase
    {
        public RPackSinexcelPowerControl(byte[] data) : base(data)
        {
            IsStart = Value == 0x5555;
        }

        public bool IsStart { get; set; }
    }
}