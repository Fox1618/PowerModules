using Hardware.DeviceSending.Interfaces;
using Hardware.DeviceSending;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.SendPackages.Uugreen;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenBase : RPackPmModulesBase, IPackUugreenBase
    {
        public RPackUugreenBase(byte[] data) : base(data)
        {
            Protocol = Helper.GetBits(data[11], 2, 4);
            MonitorAddress = (byte) (Helper.GetBits(data[11], 1, 1) << 3 | Helper.GetBits(data[10], 6, 3));
            ModuleAddress = (byte) (Helper.GetBits(data[10], 1, 5) << 2 | Helper.GetBits(data[9], 7, 2));
            ProductionDate = Helper.GetBits(data[9], 2, 5);
            SerialNumberLowerPart = (ushort) (Helper.GetBits(data[9], 1, 1) << 7 | data[8]);
            GroupAddress = Helper.GetBits(data[7], 5, 4);
            MessageType = GetMessageType(data);
            CommandType = GetCommandType(data);
            Reserve = data.SubArray(4, 2);
            Data = GetData(data);
        }

        public static byte GetMessageType(byte[] data)
        {
            return Helper.GetBits(data[7], 1, 4);
        }

        public static byte GetCommandType(byte[] data)
        {
            return data[6];
        }
        public static byte[] GetData(byte[] data)
        {
            return data.SubArray(0, 4);
        }
        public byte Protocol { get; set; }
        public byte ProductionDate { get; set; }
        public ushort SerialNumberLowerPart { get; set; }
        public byte GroupAddress { get; set; }
        public byte MessageType { get; set; }
        public byte[] Reserve { get; set; }
        public override StatusPack CheckStatus(ISPackBase sendPack)
        {
            Status = StatusPack.Unfit;
            if (!(sendPack is SPackUugreenBase pack)) return Status = StatusPack.Undefined;
            if (CommandType != pack.CommandType) return Status;
            if (pack.ModuleAddress != 0 &&pack.ModuleAddress != ModuleAddress) return Status;
            if (MessageType == (byte)MessageTypeUugreen.SetData &&
                pack.MessageType != (byte)MessageTypeUugreen.SetDataResponse) return Status;
            if (MessageType == (byte) MessageTypeUugreen.ReadDataResponse &&
                pack.MessageType != (byte) MessageTypeUugreen.ReadData) return Status;

            return Status = StatusPack.Success;
        }
    }
}