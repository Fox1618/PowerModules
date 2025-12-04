using System;
using System.Linq;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenSetLowOrHighVoltageMode : SPackUugreenBase
    {
        private uint _mode;
        public SPackUugreenSetLowOrHighVoltageMode(byte moduleAddress, bool isLow500V = true)
        {
            MessageType = (byte) MessageTypeUugreen.SetData;
            CommandType = (byte) CommandCodesUugreen.HiModeLoModeSelection;
            MonitorAddress = 1;
            ModuleAddress = moduleAddress;
            _mode = (uint)(isLow500V ? 0 : 1);
        }
        public override byte[] Data => BitConverter.GetBytes(_mode).Reverse().ToArray();
    }
}
