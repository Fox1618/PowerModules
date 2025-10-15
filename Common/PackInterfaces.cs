using Hardware.DeviceSending.Interfaces;

namespace Telemetria.Hardware.PowerModules.Common
{
    public interface IPackPModuleBase
    {
        byte MonitorAddress { get; set; }
        byte ModuleAddress { get; set; }
        byte[] Data { get; }
    }
    public interface IPackTonheBase : IPackPModuleBase 
    {
        byte Priority { get; set; }
        bool Reserve { get; set; }
        bool Dp { get; set; }
    }
    public interface IPackUugreenBase : IPackPModuleBase
    {
        byte Protocol { get; set; }
        byte ProductionDate { get; set; }
        ushort SerialNumberLowerPart { get; set; }
        byte GroupAddress { get; set; }
        byte MessageType { get; set; }
        byte[] Reserve { get; set; }
    }
    public interface IRPackPmModulesBase : IPackPModuleBase, IRPackBase
    {
    }
    public interface ISPackPmModulesBase : IPackPModuleBase, ISPackBase
    {
        byte CommandType { get; set; }
    }
}
