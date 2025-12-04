using System;
using System.Linq;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenGetLowOrHighVoltageMode : RPackUugreenBase
    {
        public RPackUugreenGetLowOrHighVoltageMode(byte[] data): base(data)
        {
            Mode = BitConverter.ToUInt32(Data.Reverse().ToArray(), 0);
        }
        public uint Mode { get; set; }
    }
}
