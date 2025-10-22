using Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe.Models;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe
{
    public class RPackTonheAcInfo : RPackTonheBase
    {
        public RPackTonheAcInfo(byte[] data) : base(data)
        {
            Info = new AcInfoTonhe(Data);
        }
        public AcInfoTonhe Info { get; set; }
    }
}
