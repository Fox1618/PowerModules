using System;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenReadCurrentCapability : RPackUugreenBase
    {
        public RPackUugreenReadCurrentCapability(byte[] data): base(data)
        {
            Current = (float)BitConverter.ToUInt16(Data, 0) / 10;
            CurrentCapability = (float) BitConverter.ToUInt16(Data, 2) / 10;
        }
        public float Current { get; set; }
        public float CurrentCapability { get; set; }
    }
}
