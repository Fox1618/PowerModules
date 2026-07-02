namespace Telemetria.Hardware.PowerModules.ReceivePackages.Megmeet
{
    public class RPackMegmeetSetVoltage : RPackMegmeetBase
    {
        public RPackMegmeetSetVoltage(byte[] data) : base(data)
        {
            Voltage = ValueFloat;
        }

        public float Voltage { get; set; }
    }
}
