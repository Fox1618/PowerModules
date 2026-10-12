using System;
using System.Collections.Generic;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Sinexcel
{
    public abstract class SPackSinexcelBase : SPackPModulesBase
    {
        public const uint ProtocolNo = 0x38;
        public const ushort MonitorAddressDefault = 0x700;
        public const byte GlobalBroadcastGroup = 31;
        public const byte BroadcastSubAddress = 0;
        public const byte NoError = 0xF0;
        public const int VoltageCoef = 10;
        public const int CurrentCoef = 1024;
        public const float ReferenceCurrent = 133.3f;

        protected SPackSinexcelBase(byte group, byte subAddress, ushort messageId, byte messageType = 0x03)
        {
            NeedAnswer = subAddress != BroadcastSubAddress;
            Group = group;
            SubAddress = subAddress;
            MessageId = messageId;
            MessageType = messageType;
            ErrorType = NoError;

            Data = new byte[8];
            Data[0] = messageType;
            Data[1] = NoError;
            Data[2] = (byte)(messageId >> 8);
            Data[3] = (byte)messageId;
        }

        public ushort MessageId { get; }
        public byte MessageType { get; }
        public byte ErrorType { get; }
        public byte Group { get; }
        public byte SubAddress { get; }

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

        private uint getCanId()
        {
            uint destinationAddress = (uint)((Group << 6) | SubAddress);
            uint sourceAddress = (uint)MonitorAddress;
            
            return (ProtocolNo << 22) |
                   (destinationAddress << 11) |
                   sourceAddress;
        }

        protected void SetInt32Value(int value)
        {
            Data[4] = (byte)(value >> 24);
            Data[5] = (byte)(value >> 16);
            Data[6] = (byte)(value >> 8);
            Data[7] = (byte)value;
        }

        protected void SetInt16Value(short value)
        {
            Data[6] = (byte)(value >> 8);
            Data[7] = (byte)value;
        }

        protected void SetVoltageValue(float voltage)
        {
            int value = (int)(voltage * VoltageCoef);
            SetInt32Value(value);
        }

        protected void SetCurrentValue(float current)
        {
            int value = (int)((current / ReferenceCurrent) * CurrentCoef);
            SetInt32Value(value);
        }

        protected void SetBitMask(uint mask)
        {
            Data[4] = (byte)(mask >> 24);
            Data[5] = (byte)(mask >> 16);
            Data[6] = (byte)(mask >> 8);
            Data[7] = (byte)mask;
        }
    }
}
