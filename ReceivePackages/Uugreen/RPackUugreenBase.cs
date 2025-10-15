using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen
{
    public class RPackUugreenBase : RPackPmModulesBase, IPackUugreenBase
    {
        public RPackUugreenBase(byte[] data) : base(data)
        {
            Protocol = Helper.GetBits(data[11], 2, 4);
            MonitorAddress = (byte) (Helper.GetBits(data[11], 1, 1) | Helper.GetBits(data[10], 6, 3));
            //ModuleAddress = 
            Data = data.SubArray(4, 8);
        }

        public byte Protocol { get; set; }
        public byte ProductionDate { get; set; }
        public ushort SerialNumberLowerPart { get; set; }
        public byte GroupAddress { get; set; }
        public byte MessageType { get; set; }
        public byte[] Reserve { get; set; }
    }
}