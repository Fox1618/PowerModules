using System;
using System.Linq;
using Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen.Models;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenGetLowOrHighVoltageMode : RPackUugreenBase
    {
        public RPackUugreenGetLowOrHighVoltageMode(byte[] data): base(data)
        {
            Mode = BitConverter.ToUInt32(Data.Reverse().ToArray(), 0);
            ModeEnum = (HiLoModeUugreen) Mode;
        }
        public uint Mode { get; set; }
        public HiLoModeUugreen ModeEnum { get; set; }

    }
}
