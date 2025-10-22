using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenSetCurrentLimitAll : SPackUugreenBase
    {
        public SPackUugreenSetCurrentLimitAll(float current)
        {
            MessageType = (byte) MessageTypeUugreen.SetData;
            CommandType = (byte) CommandCodesUugreen.IOutLimit;
            Current = current;
            NeedAnswer = false;
            MonitorAddress = 1;
        }
        public float Current { get; set; }
        public override byte[] Data => BitConverter.GetBytes((uint) (Current * 1000));
    }
}
