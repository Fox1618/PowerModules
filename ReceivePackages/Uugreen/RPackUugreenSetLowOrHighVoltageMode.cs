using System;
using System.Linq;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenSetLowOrHighVoltageMode : RPackUugreenBase
    {
        public RPackUugreenSetLowOrHighVoltageMode(byte[] data): base(data)
        {
            Mode = BitConverter.ToUInt32(Data.Reverse().ToArray(), 0);
        }
        public uint Mode { get; set; }
    }
}
