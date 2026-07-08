namespace Telemetria.Hardware.PowerModules.ReceivePackages.Sinexcel
{
    public enum SinexcelMode
    {
        Low,
        High
    }

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
                case 0xaaaa:
                    return SinexcelMode.High;
                default:
                    return SinexcelMode.High;
            }
        }
    }
}