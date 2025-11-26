using System;
using System.Linq;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenSetOutputVoltageRef : RPackUugreenBase
    {
        public RPackUugreenSetOutputVoltageRef(byte[] data): base(data)
        {
            Voltage = (float)BitConverter.ToUInt32(Data.Reverse().ToArray(), 0) / 1000;
        }
        public float Voltage { get; set; }
    }
}
