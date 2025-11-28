using System;
using System.Linq;
using Telemetria.Hardware.PowerModules.SendPackages.Uugreen;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenSetCurrentLimit : RPackUugreenBase
    {
        public RPackUugreenSetCurrentLimit(byte[] data): base(data)
        {
            Current = (float)BitConverter.ToUInt32(Data.Reverse().ToArray(), 0) / SPackUugreenBase.COEF_AV;
        }
        public float Current { get; set; }
    }
}
