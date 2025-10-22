using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenStatusFlagsAll : SPackUugreenBase
    {
        public SPackUugreenStatusFlagsAll()
        {
            MessageType = (byte) MessageTypeUugreen.ReadData;
            CommandType = (byte) CommandCodesUugreen.ModuleStatus;
            MonitorAddress = 1;
        }
        public float Current { get; set; }
        public override byte[] Data => new byte[4];
    }
}
