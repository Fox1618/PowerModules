namespace Telemetria.Hardware.PowerModules.Common
{
    public enum CommandCodesMegmeet : ushort
    {
        PowerControl = 0x0001,
        VoltageSetFloat = 0x0002,
        CurrentSetFloat = 0x0004,
        ModeSet = 0x005D,
        VoltageSet = 0x002C,
        CurrentSet = 0x002D,
        StatusRead = 0x0218,
        VoltageRead = 0x0231,
        CurrentRead = 0x0232
    }
}
