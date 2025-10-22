using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenSetOutputVoltageRefAll : SPackUugreenBase
    {
        public SPackUugreenSetOutputVoltageRefAll(float voltage)
        {
            MessageType = (byte) MessageTypeUugreen.SetData;
            CommandType = (byte) CommandCodesUugreen.VOutReference;
            Voltage = voltage;
            NeedAnswer = false;
            MonitorAddress = 1;
        }
        public float Voltage { get; set; }
        public override byte[] Data => BitConverter.GetBytes((uint) (Voltage * 1000));
    }
}
