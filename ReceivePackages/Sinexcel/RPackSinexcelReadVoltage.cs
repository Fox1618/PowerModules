namespace Telemetria.Hardware.PowerModules.ReceivePackages.Sinexcel
{
    public class RPackSinexcelReadVoltage : RPackSinexcelBase
    {
        public RPackSinexcelReadVoltage(byte[] data) : base(data)
        {
            Voltage = VoltageValue;
        }

        public float Voltage { get; set; }
    }
}