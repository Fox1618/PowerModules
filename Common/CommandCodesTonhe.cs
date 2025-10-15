namespace Telemetria.Hardware.PowerModules.Common
{
    public enum CommandCodesTonhe : byte
    {
        //Upload
        ChargingState = 0x01,
        ConfirmationStartStop = 0x02,
        AcInfo = 0x0B,
        ExtState = 0x09,
        //Download
        StartStop = 0x03,
        ParameterSetting = 0x04,
        TimingCmd = 0x05,
        InputModeConfig,
        AddressSettingSelection = 0x90,
        AddressSetting = 0x09,
        StartStopSpecModule = 0x06
    }
}
