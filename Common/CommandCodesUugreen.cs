namespace Telemetria.Hardware.PowerModules.Common
{
    public enum CommandCodesUugreen : byte
    {
        Vout = 0,
        IOutSlow = 1,
        VOutReference = 2,
        IOutLimit = 3,
        ShutDownDcDc = 4,
        ReadSn = 5,
        ModuleStatus = 8,
        Vab = 20,
        Vbc = 21,
        Vca = 22,
        VFanReference = 26,
        TIn = 30,
        IOutFastest = 47,
        IOutFast = 48,
        GroupAddress = 89,
        HiModeLoModeSelection = 95,
        HiModeLoModeStatus = 96,
        VOutFast = 98,
        TrueHiLoStatus = 101,
        CurrentCapability = 104,
        CurrentAndCapability = 114
    }

    public enum MessageTypeUugreen : byte
    {
        SetData = 0,
        SetDataResponse = 1,
        ReadData = 2,
        ReadDataResponse = 3,
        ReadDataSerialNumberResponse = 4,
        AllSetData = 11,
        AllSetDataResponse = 12
    }
}
