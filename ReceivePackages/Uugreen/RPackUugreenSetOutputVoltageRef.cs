using System;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenSetOutputVoltageRef : RPackUugreenBase
    {
        public RPackUugreenSetOutputVoltageRef(byte[] data): base(data)
        {
            Voltage = (float)BitConverter.ToUInt32(Data, 0) / 1000;
        }
        public float Voltage { get; set; }
    }
}
