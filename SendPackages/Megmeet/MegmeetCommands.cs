namespace Telemetria.Hardware.PowerModules.SendPackages.Megmeet
{
    internal static class MegmeetCommands
    {
        public const ushort PowerControl = 0x0001;
        public const ushort VoltageSetFloat = 0x0002;
        public const ushort CurrentSetFloat = 0x0004;
        public const ushort StatusRead = 0x0218;
        public const ushort VoltageRead = 0x0231;
        public const ushort CurrentRead = 0x0232;
        public const ushort ModeSet = 0x005D;
        public const ushort VoltageSet = 0x002C;
        public const ushort CurrentSet = 0x002D;
    }
}
