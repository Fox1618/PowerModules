using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Tonhe
{
    public class SPackTonheParamSetting : SPackTonheBase
    {
        public SPackTonheParamSetting(byte address, float voltage, float current)
        {
            CommandType = (byte) CommandCodesTonhe.ParameterSetting;
            Voltage = voltage;
            Current = current;
            ModuleAddress = byte.MaxValue;
            NeedAnswer = true;
        }
        public byte[] Addresses { get; set; } = { byte.MaxValue, byte.MaxValue, byte.MaxValue};
        public byte Command { get; set; }
        public float Voltage { get; set; }
        public float Current { get; set; }

        public override byte[] Data
        {
            get
            {
                var data = new byte[8];
                for (var i = 0; i < Addresses.Length; i++)
                {
                    data[i] = Addresses[i];       
                }
                data[3] = Command;
                var voltageBytes = BitConverter.GetBytes((ushort)Voltage*10);
                for (var i = 0; i < voltageBytes.Length; i++)
                {
                    data[4 + i] = voltageBytes[i];
                }
                var currentBytes = BitConverter.GetBytes((ushort)Current * 10);
                for (var i = 0; i < currentBytes.Length; i++)
                {
                    data[4 + i] = currentBytes[i];
                }
                return data;
            }
        }
    }
}
