using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenReadOutputCurrentAll : SPackUugreenBase
    {
        public SPackUugreenReadOutputCurrentAll()
        {
            MessageType = (byte) MessageTypeUugreen.ReadData;
            CommandType = (byte) CommandCodesUugreen.IOutSlow;
            MonitorAddress = 1;
        }
        public float Voltage { get; set; }
        public override byte[] Data => new byte[4];
    }
}
