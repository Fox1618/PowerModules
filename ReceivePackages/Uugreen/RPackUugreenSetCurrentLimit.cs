using System;
using System.Linq;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenSetCurrentLimit : RPackUugreenBase
    {
        public RPackUugreenSetCurrentLimit(byte[] data): base(data)
        {
            Current = (float)BitConverter.ToUInt32(Data.Reverse().ToArray(), 0) / 1000;
        }
        public float Current { get; set; }
    }
}
