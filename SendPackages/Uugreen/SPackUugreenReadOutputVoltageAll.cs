using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenReadOutputVoltageAll : SPackUugreenBase
    {
        public SPackUugreenReadOutputVoltageAll()
        {
            MessageType = (byte) MessageTypeUugreen.ReadData;
            CommandType = (byte) CommandCodesUugreen.Vout;
            MonitorAddress = 1;
        }
        public float Current { get; set; }
        public override byte[] Data => new byte[4];
    }
}
