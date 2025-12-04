using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenGetLowOrHighVoltageMode : SPackUugreenBase
    {
        public SPackUugreenGetLowOrHighVoltageMode(byte moduleAddress)
        {
            MessageType = (byte) MessageTypeUugreen.ReadData;
            CommandType = (byte) CommandCodesUugreen.HiModeLoModeStatus;
            MonitorAddress = 1;
            ModuleAddress = moduleAddress;
        }
        public float Current { get; set; }
        public override byte[] Data => new byte[] { 0, 0, 0, 0};
    }
}
