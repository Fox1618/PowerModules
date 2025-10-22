using Hardware.DeviceSending.Interfaces;
using Hardware.DeviceSending;
using Telemetria.Hardware.PowerModules.SendPackages.Tonhe;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe
{
    public class RPackTonheBase : RPackPmModulesBase
    {
        public RPackTonheBase(byte[] data) : base(data)
        {
            Dp = Helper.GetBit(data[0], 1);
            Reserve = Helper.GetBit(data[0], 2);
            Priority = Helper.GetBits(data[0], 3, 3);
            CommandType = data[1];
            MonitorAddress = data[2];
            ModuleAddress = data[3];
            Data = data.SubArray(4, 8);
        }

        public bool Dp { get; set; }
        public byte Priority { get; set; }
        public bool Reserve { get; set; }
        public override StatusPack CheckStatus(ISPackBase sendPack)
        {
            Status = StatusPack.Unfit;
            if (!(sendPack is SPackTonheBase pack)) return Status = StatusPack.Undefined;
            if (CommandType != pack.CommandType) return Status;
            return Status = StatusPack.Success;
        }
    }
}