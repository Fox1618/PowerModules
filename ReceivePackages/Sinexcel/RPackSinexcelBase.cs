using Hardware.DeviceSending;
using Hardware.DeviceSending.Interfaces;
using Telemetria.Hardware.PowerModules.SendPackages.Sinexcel;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Sinexcel
{
    public class RPackSinexcelBase : RPackPmModulesBase
    {
        public RPackSinexcelBase(byte[] data) : base(data)
        {
            CanId = ReadUInt32(data, 0);
            ProtocolCode = (byte)((CanId >> 22) & 0x7F);
            DestinationAddress = (ushort)((CanId >> 11) & 0x7FF);
            SourceAddress = (ushort)(CanId & 0x7FF);

            MonitorAddress = (byte)DestinationAddress;
            Group = (byte)((SourceAddress >> 6) & 0x1F);
            SubAddress = (byte)(SourceAddress & 0x3F);
            Data = data.SubArray(4, 8);

            MessageType = (byte)Data[0];
            ErrorType = (byte)Data[1];
            MessageId = ReadUInt16(Data, 2);
            Value = ReadUInt32(Data, 4);
        }

        public uint CanId { get; set; }
        public byte ProtocolCode { get; set; }
        public ushort DestinationAddress { get; set; }
        public ushort SourceAddress { get; set; }
        public byte Group { get; set; }
        public byte SubAddress { get; set; }
        public byte MessageType { get; set; }
        public byte ErrorType { get; set; }
        public ushort MessageId { get; set; }
        public uint Value { get; set; }

        public float VoltageValue => (float)Value / SPackSinexcelBase.VoltageCoef;
        public float CurrentValue => (float)Value / SPackSinexcelBase.CurrentCoef * SPackSinexcelBase.ReferenceCurrent;

        public override StatusPack CheckStatus(ISPackBase sendPack)
        {
            Status = StatusPack.Unfit;
            if (!(sendPack is SPackSinexcelBase pack)) return Status = StatusPack.Undefined;
            if (ProtocolCode != SPackSinexcelBase.ProtocolNo) return Status;
            if (MessageId != pack.MessageId) return Status;
            if (pack.Group != SPackSinexcelBase.GlobalBroadcastGroup &&
                pack.SubAddress != SPackSinexcelBase.BroadcastSubAddress &&
                (Group != pack.Group || SubAddress != pack.SubAddress)) return Status;
            if (ErrorType != SPackSinexcelBase.NoError) return Status = StatusPack.Fail;
            return Status = StatusPack.Success;
        }

        public static ushort ReadMessageId(byte[] data)
        {
            return (ushort)(data[6] << 8 | data[7]);
        }

        protected static ushort ReadUInt16(byte[] data, int startIndex)
        {
            return (ushort)(data[startIndex] << 8 | data[startIndex + 1]);
        }

        protected static uint ReadUInt32(byte[] data, int startIndex)
        {
            return (uint)(data[startIndex] << 24 |
                          data[startIndex + 1] << 16 |
                          data[startIndex + 2] << 8 |
                          data[startIndex + 3]);
        }
    }
}