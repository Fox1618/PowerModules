using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Megmeet
{
    public class RPackMegmeetReadStatus : RPackMegmeetBase
    {
        public RPackMegmeetReadStatus(byte[] data) : base(data)
        {
            State = new StateBase(Data);
        }

        public StateBase State { get; set; }
    }
}
