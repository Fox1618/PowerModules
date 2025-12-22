using System;
using System.Linq;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen.Models;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenSetLowOrHighVoltageMode : SPackUugreenBase
    {
        private uint _mode;
        public SPackUugreenSetLowOrHighVoltageMode(byte moduleAddress, HiLoModeUugreen mode)
        {
            MessageType = (byte) MessageTypeUugreen.SetData;
            CommandType = (byte) CommandCodesUugreen.HiModeLoModeSelection;
            MonitorAddress = 1;
            ModuleAddress = moduleAddress;
            _mode = (uint)mode;
        }
        public override byte[] Data => BitConverter.GetBytes(_mode).Reverse().ToArray();
    }
}
