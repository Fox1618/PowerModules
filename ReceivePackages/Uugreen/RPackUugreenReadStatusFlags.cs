using System;
using Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen.Models;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenReadStatusFlags : RPackUugreenBase
    {
        public RPackUugreenReadStatusFlags(byte[] data): base(data)
        {
            Flags = new StatusFlagsUugreen(data);
        }
        public StatusFlagsUugreen Flags { get; set; }
    }
}
