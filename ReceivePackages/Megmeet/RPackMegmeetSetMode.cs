using Telemetria.Hardware.PowerModules.SendPackages.Megmeet;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Megmeet
{
    public class RPackMegmeetSetMode : RPackMegmeetBase
    {
        public RPackMegmeetSetMode(byte[] data) : base(data)
        {
            Mode = GetMode(Value);
        }

        public MegmeetMode Mode { get; set; }

        private static MegmeetMode GetMode(uint value)
        {
            switch (value)
            {
                case 0x1111:
                    return MegmeetMode.Low;
                case 0x2222:
                    return MegmeetMode.High;
                default:
                    return MegmeetMode.Auto;
            }
        }
    }
}
