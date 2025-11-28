using System;
using System.Collections.Generic;
using System.Linq;
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
            MonitorAddress = 0xA0;
        }

        public bool Dp { get; set; }
        public byte Priority { get; set; }
        public bool Reserve { get; set; }

        public override byte[] ToByte()
        {
            var res = new List<byte>();
            if (Prefix != null)
                res.Add(Prefix.Value);
            res.Add((byte)(Convert.ToByte(Dp) | (Convert.ToByte(Reserve) << 1) | (Convert.ToByte(Priority) << 2)));
            res.Add(CommandType);
            res.Add(ModuleAddress);
            res.Add(MonitorAddress);
            res.AddRange(Data);
            return res.ToArray();
            /*var data = new byte[12];
            var dataMain = Data;
            for (var i = 0; i < dataMain.Length; i++)
            {
                data[11 - i] = dataMain[i];
            }

            data[3] = MonitorAddress;
            data[2] = ModuleAddress;
            data[1] = CommandType;
            data[0] = (byte) (Convert.ToByte(Dp) | (Convert.ToByte(Reserve) << 1) | (Convert.ToByte(Dp) << 2));
            return data;*/
        }

        protected (byte multipleAddress, byte[] addresses) _getAddress(byte address)
        {
            var pow = (uint)Math.Pow(2, address % 24 - 1);
            var bytes = BitConverter.GetBytes(pow);
            //var res = bytes.ToArray().SubArray(0, 3);
            return ((byte) (address / 24),
                BitConverter.GetBytes((uint)Math.Pow(2, address % 24 - 1)).ToArray().SubArray(0, 3));
        }
    }
}
