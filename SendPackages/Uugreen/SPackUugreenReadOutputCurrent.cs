using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenReadOutputCurrent : SPackUugreenBase
    {
        public SPackUugreenReadOutputCurrent(byte moduleAddress)
        {
            MessageType = (byte) MessageTypeUugreen.ReadData;
            CommandType = (byte) CommandCodesUugreen.IOutSlow;
            MonitorAddress = 1;
            ModuleAddress = moduleAddress;
            MonitorAddress = 1;
        }
        public float Current { get; set; }
        public override byte[] Data => new byte[4];
    }
}
