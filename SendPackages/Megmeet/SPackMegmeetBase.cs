using System;
using System.Collections.Generic;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public abstract class SPackMegmeetBase : SPackPModulesBase
    {
        public const uint ProtocolNo = 0x060;
        public const byte MonitorAddressDefault = 0xF0;
        public const byte BroadcastAddress = 0xFF;
        public const byte RequestSingleData = 0x01;
        public const byte NoError = 0xF0;
        public const int COEF_AV = 1000;

        protected SPackMegmeetBase(byte moduleAddress, ushort command)
        {
            NeedAnswer = moduleAddress != BroadcastAddress;
            MonitorAddress = MonitorAddressDefault;
            ModuleAddress = moduleAddress;
            Command = command;
            CommandType = (byte)(command & 0xFF);
            Data = new byte[8];
            Data[0] = RequestSingleData;
            Data[1] = NoError;
            Data[2] = (byte)(command >> 8);
            Data[3] = (byte)command;
        }

        public ushort Command { get; }

        public override byte[] ToByte()
        {
            var result = new List<byte>();
            if (Prefix != null)
                result.Add(Prefix.Value);

            var canId = getCanId();
            result.Add((byte)(canId >> 24));
            result.Add((byte)(canId >> 16));
            result.Add((byte)(canId >> 8));
            result.Add((byte)canId);
            result.AddRange(Data);
            return result.ToArray();
        }

        protected void SetFloatValue(float value)
        {
            SetUInt32Value((uint)(value * COEF_AV));
        }

        protected void SetUInt16Value(ushort value)
        {
            Data[6] = (byte)(value >> 8);
            Data[7] = (byte)value;
        }

        protected void SetUInt32Value(uint value)
        {
            Data[4] = (byte)(value >> 24);
            Data[5] = (byte)(value >> 16);
            Data[6] = (byte)(value >> 8);
            Data[7] = (byte)value;
        }

        private uint getCanId()
        {
            return (ProtocolNo << 20) |
                   (1u << 19) |
                   ((uint)ModuleAddress << 11) |
                   ((uint)MonitorAddress << 3) |
                   0x03;
        }
    }
}
