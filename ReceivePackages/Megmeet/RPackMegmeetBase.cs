using Hardware.DeviceSending;
using Hardware.DeviceSending.Interfaces;
using Telemetria.Hardware.PowerModules.SendPackages.Megmeet;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Megmeet
{
    public class RPackMegmeetBase : RPackPmModulesBase
    {
        public RPackMegmeetBase(byte[] data) : base(data)
        {
            CanId = ReadUInt32(data, 0);
            ProtocolNo = (ushort)((CanId >> 20) & 0x1FF);
            Ptp = ((CanId >> 19) & 0x01) == 1;
            DestinationAddress = (byte)((CanId >> 11) & 0xFF);
            SourceAddress = (byte)((CanId >> 3) & 0xFF);
            Reserve = (byte)(CanId & 0x07);

            MonitorAddress = DestinationAddress;
            ModuleAddress = SourceAddress;
            Data = data.SubArray(4, 8);

            MessageType = Data[0];
            ErrorType = Data[1];
            Command = ReadUInt16(Data, 2);
            CommandType = (byte)(Command & 0xFF);
            Value = ReadUInt32(Data, 4);
        }

        public uint CanId { get; set; }
        public ushort ProtocolNo { get; set; }
        public bool Ptp { get; set; }
        public byte DestinationAddress { get; set; }
        public byte SourceAddress { get; set; }
        public byte Reserve { get; set; }
        public byte MessageType { get; set; }
        public byte ErrorType { get; set; }
        public ushort Command { get; set; }
        public uint Value { get; set; }

        public float ValueFloat => (float)Value / SPackMegmeetBase.COEF_AV;

        public override StatusPack CheckStatus(ISPackBase sendPack)
        {
            Status = StatusPack.Unfit;
            if (!(sendPack is SPackMegmeetBase pack)) return Status = StatusPack.Undefined;
            if (ProtocolNo != SPackMegmeetBase.ProtocolNo) return Status;
            if (MessageType != SPackMegmeetBase.RequestSingleData) return Status;
            if (Command != pack.Command) return Status;
            if (pack.ModuleAddress != SPackMegmeetBase.BroadcastAddress && ModuleAddress != pack.ModuleAddress) return Status;
            if (ErrorType != SPackMegmeetBase.NoError) return Status = StatusPack.Fail;
            return Status = StatusPack.Success;
        }

        public static ushort ReadCommand(byte[] data)
        {
            return ReadUInt16(data, 6);
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
