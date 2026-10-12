namespace Telemetria.Hardware.PowerModules.ReceivePackages.Megmeet
{
    public class RPackMegmeetReadVoltage : RPackMegmeetBase
    {
        public RPackMegmeetReadVoltage(byte[] data) : base(data)
        {
            Voltage = ValueFloat;
        }

        public float Voltage { get; set; }
    }
}
