using System;
using System.Linq;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenSetCurrentLimit : SPackUugreenBase
    {
        public SPackUugreenSetCurrentLimit(float current, byte moduleAddress)
        {
            MessageType = (byte) MessageTypeUugreen.SetData;
            CommandType = (byte) CommandCodesUugreen.IOutLimit;
            Current = current;
            MonitorAddress = 1;
            ModuleAddress = moduleAddress;
        }
        public float Current { get; set; }
        public override byte[] Data => BitConverter.GetBytes((uint) (Current * 1000)).Reverse().ToArray();
    }
}
