using System;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen.Models;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenAllSetData : SPackUugreenBase
    {
        public SPackUugreenAllSetData(byte moduleAddress, float current, float batteryVoltage, float voltage, HiLoModeOnOffUugreen mode = null)
        {
            MessageType = (byte) MessageTypeUugreen.AllSetData;
            CommandType = (byte) CommandCodesUugreen.Vout;
            MonitorAddress = 1;
            ModuleAddress = moduleAddress;
            Current = current;
            VoltageBattery = batteryVoltage;
            Voltage = voltage;
            Reserve = BitConverter.GetBytes((ushort) (Current * 10));
            MessageType = mode?.ToByte() ?? 0;
        }

        public float Current { get; }
        public float Voltage { get; set; }
        public float VoltageBattery { get; set; }

        public override byte[] Data
        {
            get
            {
                var fWord = BitConverter.GetBytes((ushort) (VoltageBattery * 10));
                var sWord = BitConverter.GetBytes((ushort) (Voltage * 10));
                return new[] {fWord[0], fWord[1], sWord[0], sWord[1]};
            }
        }
    }
}
