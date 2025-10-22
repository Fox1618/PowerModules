using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe.Models
{
    public class AcInfoTonhe
    {
        public AcInfoTonhe(byte[] data)
        {
            APhaseVoltage = (float)BitConverter.ToUInt16(data, 0) / 10;
            BPhaseVoltage = (float)BitConverter.ToUInt16(data, 2) / 10;
            CPhaseVoltage = (float)BitConverter.ToUInt16(data, 4) / 10;
            AmbientTemp = (float)BitConverter.ToUInt16(data, 6) / 10;
        }
        public float APhaseVoltage { get; set; }
        public float BPhaseVoltage { get; set; }
        public float CPhaseVoltage { get; set; }
        public float AmbientTemp { get; set; }
    }
}
