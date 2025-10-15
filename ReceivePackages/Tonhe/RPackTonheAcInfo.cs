using Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe;
using Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe.Models;

namespace Telemetria.Hardware.PowerModules.ReceivePackages
{
    public class RPackTonheAcInfo : RPackTonheBase
    {
        public RPackTonheAcInfo(byte[] data) : base(data)
        {
            Info = new AcInfo(Data);
        }
        public AcInfo Info { get; set; }
    }
}
