namespace Telemetria.Hardware.PowerModules.Common
{
    public enum CommandCodesSinexcel : ushort
    {
        MessageTypeQuery = 0x50,
        MessageTypeResponse = 0x41,
        MessageTypeSetup = 0x03,
        MessageTypeSetupResponse = 0x43,

        ErrorTypeNoError = 0xF0,
        ErrorTypeInvalidAddress = 0xF1,
        ErrorTypeInvalidCommand = 0xF2,
        ErrorTypeDataCheckError = 0xF3,
        ErrorTypeAddressIdentifying = 0xF4,

        QueryComprehensive = 0x0202,
        QueryOnlineInfoLow = 0xffff,
        QueryOnlineInfoHigh = 0xfffe,

        QueryDcOperatingStatus = 0x0202,
        QueryDcStatusFault = 0x0203,
        QueryAcStatusFault = 0x0204,
        QueryOutputVoltage = 0x0205,
        QueryOutputCurrent = 0x0206,
        QuerySoftwareVersion = 0x020a,

        PowerControlLow = 0x0230,
        PowerControlHigh = 0x0232,
        SetVoltageMode = 0x0233,
        SetOutputVoltage = 0x0021,
        SetOutputCurrent = 0x0022,
        SetGroupNumber = 0x001a,

        ProtocolCode = 0x38,
        OnlineInfoProtocolCode = 0x2C
    }
}
