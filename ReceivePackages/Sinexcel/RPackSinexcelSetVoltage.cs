namespace Telemetria.Hardware.PowerModules.ReceivePackages.Sinexcel
{
    public class RPackSinexcelSetVoltage : RPackSinexcelBase
    {
        public RPackSinexcelSetVoltage(byte[] data) : base(data)
        {
            Voltage = VoltageValue;
        }

        public float Voltage { get; set; }
    }
}