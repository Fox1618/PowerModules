using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenShutdown : SPackUugreenBase
    {
        public SPackUugreenShutdown(byte moduleAddress)
        {
            MessageType = (byte) MessageTypeUugreen.SetData;
            CommandType = (byte) CommandCodesUugreen.ShutDownDcDc;
            MonitorAddress = 1;
            ModuleAddress = moduleAddress;
        }
        public override byte[] Data => new byte[]{0, 0, 0, 1};
    }
}
