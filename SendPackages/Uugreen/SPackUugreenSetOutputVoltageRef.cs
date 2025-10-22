using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenSetOutputVoltageRef : SPackUugreenBase
    {
        public SPackUugreenSetOutputVoltageRef(float voltage, byte moduleAddress)
        {
            MessageType = (byte) MessageTypeUugreen.SetData;
            CommandType = (byte) CommandCodesUugreen.VOutReference;
            Voltage = voltage;
            MonitorAddress = 1;
            ModuleAddress = moduleAddress;
        }
        public float Voltage { get; set; }
        public override byte[] Data => BitConverter.GetBytes((uint) (Voltage * 1000));
    }
}
