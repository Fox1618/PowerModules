using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenReadStatusFlags : SPackUugreenBase
    {
        public SPackUugreenReadStatusFlags(byte moduleAddress)
        {
            MessageType = (byte) MessageTypeUugreen.ReadData;
            CommandType = (byte) CommandCodesUugreen.ModuleStatus;
            MonitorAddress = 1;
            ModuleAddress = moduleAddress;
        }
        public override byte[] Data => new byte[4];
    }
}
