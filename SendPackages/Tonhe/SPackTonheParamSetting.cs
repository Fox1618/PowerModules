using System;
using System.Linq;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Tonhe
{
    public class SPackTonheParamSetting : SPackTonheBase
    {
        public SPackTonheParamSetting(byte? address, float voltage, float current)
        {
            if (address != null)
            {
                (GroupAddress, Addresses) = _getAddress(address.Value);
            }
            CommandType = (byte) CommandCodesTonhe.ParameterSetting;
            Voltage = voltage;
            Current = current;
            ModuleAddress = byte.MaxValue;
            Priority = 4;
        }
        public byte[] Addresses { get; set; } = { byte.MaxValue, byte.MaxValue, byte.MaxValue};
        public byte GroupAddress { get; set; }
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
                data[3] = GroupAddress;
                var voltageBytes = BitConverter.GetBytes((ushort)(Voltage * 10)).ToArray();
                for (var i = 0; i < voltageBytes.Length; i++)
                {
                    data[4 + i] = voltageBytes[i];
                }
                var currentBytes = BitConverter.GetBytes((ushort)(Current * 10)).ToArray();
                for (var i = 0; i < currentBytes.Length; i++)
                {
                    data[6 + i] = currentBytes[i];
                }
                return data;
            }
        }
    }
}
