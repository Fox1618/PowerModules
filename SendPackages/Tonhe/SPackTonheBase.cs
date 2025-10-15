using System;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Tonhe
{
    public class SPackTonheBase : SPackPModulesBase, IPackTonheBase
    {
        public SPackTonheBase()
        {
            Dp = false;
            Reserve = false;
            Priority = 6;
        }
        public bool Dp { get; set; }
        public byte Priority { get; set; }
        public bool Reserve { get; set; }

        public override byte[] ToByte()
        {
            var data = new byte[12];
            for (var i = 0; i < Data.Length; i++)
            {
                data[11 - i] = Data[i];
            }
            data[3] = MonitorAddress;
            data[2] = ModuleAddress;
            data[1] = CommandType;
            data[0] = (byte) (Convert.ToByte(Dp) | (Convert.ToByte(Reserve) << 1) | (Convert.ToByte(Dp) << 2));
            return data;
        }
    }
}