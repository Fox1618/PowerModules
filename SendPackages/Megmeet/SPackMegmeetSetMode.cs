namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    public sealed class SPackMegmeetSetMode : SPackMegmeetBase
    {
        public SPackMegmeetSetMode(byte moduleAddress, MegmeetMode mode) : base(moduleAddress, MegmeetCommands.ModeSet)
        {
            Mode = mode;
            SetUInt16Value(getModeValue(mode));
        }

        public MegmeetMode Mode { get; }

        private static ushort getModeValue(MegmeetMode mode)
        {
            switch (mode)
            {
                case MegmeetMode.Low:
                    return 0x1111;
                case MegmeetMode.High:
                    return 0x2222;
                default:
                    return 0x0000;
            }
        }
    }
}
