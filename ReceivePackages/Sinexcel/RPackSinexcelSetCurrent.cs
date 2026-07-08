namespace Telemetria.Hardware.PowerModules.ReceivePackages.Sinexcel
{
    public class RPackSinexcelSetCurrent : RPackSinexcelBase
    {
        public RPackSinexcelSetCurrent(byte[] data) : base(data)
        {
            Current = CurrentValue;
        }

        public float Current { get; set; }
    }
}