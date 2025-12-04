using System;
using Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen.Models;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenAllSetData : RPackUugreenBase
    {
        public RPackUugreenAllSetData(byte[] data) : base(data)
        {
            Current = (float) BitConverter.ToUInt16(Reserve, 0) / 10;
            VoltageBattery = (float) BitConverter.ToUInt16(Data, 0) / 10;
            Voltage = (float) BitConverter.ToUInt16(Data, 2) / 10;
            HiLoModeOnOff = new HiLoModeOnOffUugreen(CommandType);
        }
        public float Current { get; }
        public float Voltage { get; }
        public float VoltageBattery { get; }
        public HiLoModeOnOffUugreen HiLoModeOnOff { get; set; }
    }
}
