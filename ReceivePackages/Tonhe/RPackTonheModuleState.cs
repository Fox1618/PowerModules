using Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe.Models;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe
{
    public class RPackTonheModuleState : RPackTonheBase
    {
        public RPackTonheModuleState(byte[] data) : base(data)
        {
            State = new ModuleStateTonhe(Data);
        }
        public ModuleStateTonhe State { get; set; }
    }
}
