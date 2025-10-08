using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardware.DeviceSending;

namespace Telemetria.Hardware.PowerModules.SendPackages.Tonhe
{
    public class SPackTonheBase : SPackBase
    {
        public byte Priority { get; set; }
        public bool Reservation { get; set; }
        public bool Dp { get; set; }
        public byte CodeCmd { get; set; }
        public byte TargetAddress { get; set; }
        public byte SourceAddress { get; set; }
        List<byte> Data { get; set; }

        public override byte[] ToByte()
        {
            throw new NotImplementedException();
        }
    }
}
