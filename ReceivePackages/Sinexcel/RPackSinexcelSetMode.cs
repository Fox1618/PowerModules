using System.Diagnostics;
using Telemetria.Hardware.PowerModules.SendPackages.Sinexcel;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Sinexcel
{
    public class RPackSinexcelSetMode : RPackSinexcelBase
    {
        public RPackSinexcelSetMode(byte[] data) : base(data)
        {
            Mode = GetMode(Value);
        }

        public SinexcelMode Mode { get; set; }

        private static SinexcelMode GetMode(uint value)
        {
            switch (value)
            {
                case 0x5555:
                    return SinexcelMode.Low;
                case 0xAAAA:
                    return SinexcelMode.High;
                default:
                    Debug.WriteLine($"[Sinexcel] Mode is incorrect: {value:X4}");
                    return SinexcelMode.Unknown;
            }
        }
    }
}