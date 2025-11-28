using System;
using System.Linq;
using Telemetria.Hardware.PowerModules.SendPackages.Uugreen;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenReadOutputVoltage : RPackUugreenBase
    {
        public RPackUugreenReadOutputVoltage(byte[] data): base(data)
        {
            Voltage = (float)BitConverter.ToUInt32(Data.Reverse().ToArray(), 0) / SPackUugreenBase.COEF_AV;
        }
        public float Voltage { get; set; }
    }
}
