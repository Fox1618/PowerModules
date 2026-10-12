namespace Telemetria.Hardware.PowerModules.ReceivePackages.Sinexcel
{
    public class RPackSinexcelReadCurrent : RPackSinexcelBase
    {
        public RPackSinexcelReadCurrent(byte[] data) : base(data)
        {
            Current = CurrentValue;
        }

        public float Current { get; set; }
    }
}