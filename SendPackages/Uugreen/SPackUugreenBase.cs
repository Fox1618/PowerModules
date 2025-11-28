using System.Collections.Generic;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenBase : SPackPModulesBase, IPackUugreenBase
    {
        public const int COEF_AV = 1000;
        public SPackUugreenBase()
        {
            NeedAnswer = true;
            ModuleAddress = 0;
        }
        public byte Protocol { get; set; } = 1;
        public byte ProductionDate { get; set; } = 0;
        public ushort SerialNumberLowerPart { get; set; } = 0;
        public byte GroupAddress { get; set; } = 1;
        public byte MessageType { get; set; }
        public byte[] Reserve { get; set; } = new byte[2];
        public override byte[] ToByte()
        {
            var res = Helper.GetBits(32, 6, 3);
            var b1 = (byte)(Helper.GetBits(Protocol, 1, 4) << 1 | Helper.GetBits(MonitorAddress, 4, 1));
            var b2 = (byte)(Helper.GetBits(MonitorAddress, 1, 3) << 5 | Helper.GetBits(ModuleAddress, 3, 5));
            var b3 = (byte)(Helper.GetBits(ModuleAddress, 1, 2) << 6 | Helper.GetBits(ProductionDate, 1, 5) << 1 |
                     Helper.GetBits(SerialNumberLowerPart, 8, 1));
            var b4 = (byte)Helper.GetBits(SerialNumberLowerPart, 1, 8);
            var b5 = (byte) (Helper.GetBits(GroupAddress, 1, 4) << 4 | Helper.GetBits(MessageType, 1, 4));
            var data = new List<byte> {b1, b2, b3, b4, b5, CommandType};
            if (Prefix != null) {
                data.Insert(0, Prefix.Value);
            }
            data.AddRange(Reserve);
            data.AddRange(Data);
            return data.ToArray();
        }
    }
}
