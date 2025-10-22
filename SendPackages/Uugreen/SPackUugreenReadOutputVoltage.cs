using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenReadOutputVoltage : SPackUugreenBase
    {
        public SPackUugreenReadOutputVoltage(byte moduleAddress)
        {
            MessageType = (byte) MessageTypeUugreen.ReadData;
            CommandType = (byte) CommandCodesUugreen.Vout;
            MonitorAddress = 1;
            ModuleAddress = moduleAddress;
        }
        public float Current { get; set; }
        public override byte[] Data => new byte[4];
    }
}
