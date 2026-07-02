namespace Telemetria.Hardware.PowerModules.ReceivePackages.Megmeet
{
    public class RPackMegmeetSetCurrent : RPackMegmeetBase
    {
        public RPackMegmeetSetCurrent(byte[] data) : base(data)
        {
            Current = ValueFloat;
        }

        public float Current { get; set; }
    }
}
