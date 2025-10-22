using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Uugreen
{
    public class SPackUugreenShutdownAll : SPackUugreenBase
    {
        public SPackUugreenShutdownAll()
        {
            MessageType = (byte) MessageTypeUugreen.SetData;
            CommandType = (byte) CommandCodesUugreen.ShutDownDcDc;
            MonitorAddress = 1;
            NeedAnswer = false;
        }
        public override byte[] Data => new byte[]{0, 0, 0, 1};
    }
}
