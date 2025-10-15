using System.Collections.Generic;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenBase : SPackPModulesBase, IPackUugreenBase
    {
        public byte Protocol { get; set; } = 1;
        public byte ProductionDate { get; set; } = 0;
        public ushort SerialNumberLowerPart { get; set; } = 0;
        public byte GroupAddress { get; set; }
        public byte MessageType { get; set; }
        public byte[] Reserve { get; set; } = new byte[2];
        public override byte[] ToByte()
        {
            var b1 = (byte)(Helper.GetBits(Protocol, 0, 4) << 1 | Helper.GetBits(ModuleAddress, 4, 1));
            var b2 = (byte)(Helper.GetBits(ModuleAddress, 0, 3) << 5 | Helper.GetBits(MonitorAddress, 1, 5));
            var b3 = (byte)(Helper.GetBits(MonitorAddress, 6, 2) << 6 | Helper.GetBits(ProductionDate, 0, 5) << 1 |
                     Helper.GetBits(SerialNumberLowerPart, 9, 1));
            var b4 = (byte)Helper.GetBits(SerialNumberLowerPart, 1, 8);
            var b5 = (byte) (Helper.GetBits(GroupAddress, 1, 4) << 4 | Helper.GetBits(MessageType, 5, 4));
            var data = new List<byte> {b1, b2, b3, b4, b5, CommandType};
            data.AddRange(Reserve);
            data.AddRange(Data);
            return base.ToByte();
        }
    }
}
