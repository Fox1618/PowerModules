using System;
using System.Linq;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen.Models;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenAllSetData : SPackUugreenBase
    {
        public SPackUugreenAllSetData(byte moduleAddress, float voltage, float batteryVoltage, float current, HiLoModeOnOffUugreen mode = null)
        {
            MessageType = (byte) MessageTypeUugreen.AllSetData;
            CommandType = mode?.ToByte() ?? 0;
            MonitorAddress = 1;
            ModuleAddress = moduleAddress;
            Current = current;
            VoltageBattery = batteryVoltage;
            Voltage = voltage;
            Reserve = BitConverter.GetBytes((ushort) (Current * 10)).Reverse().ToArray();
        }

        public float Current { get; }
        public float Voltage { get; set; }
        public float VoltageBattery { get; set; }

        public override byte[] Data
        {
            get
            {

                var fWord = BitConverter.GetBytes((ushort) (VoltageBattery * 10)).Reverse().ToArray();
                var sWord = BitConverter.GetBytes((ushort) (Voltage * 10)).Reverse().ToList();
                return new[] {fWord[0], fWord[1], sWord[0], sWord[1]};
            }
        }
    }
}
