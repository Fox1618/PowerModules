namespace Telemetria.Hardware.PowerModules.ReceivePackages.Megmeet
{
    public class RPackMegmeetReadCurrent : RPackMegmeetBase
    {
        public RPackMegmeetReadCurrent(byte[] data) : base(data)
        {
            Current = ValueFloat;
        }

        public float Current { get; set; }
    }
}
