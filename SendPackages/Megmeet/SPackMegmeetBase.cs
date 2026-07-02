using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public abstract class SPackMegmeetBase : SPackPModulesBase
    {
        private const uint ProtocolNo = 0x060;
        private const byte MonitorAddressDefault = 0xF0;
        private const byte BroadcastAddress = 0xFF;
        private const byte RequestSingleData = 0x01;
        private const byte NoError = 0xF0;
        private const int CoefAv = 1000;

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
            var result = new byte[12];
            var canId = getCanId();
            result[0] = (byte)(canId >> 24);
            result[1] = (byte)(canId >> 16);
            result[2] = (byte)(canId >> 8);
            result[3] = (byte)canId;
            Array.Copy(Data, 0, result, 4, Data.Length);
            return result;
        }

        protected void SetFloatValue(float value)
        {
            SetUInt32Value((uint)(value * CoefAv));
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
