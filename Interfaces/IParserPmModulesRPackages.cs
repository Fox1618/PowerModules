using Hardware.DeviceSending.Interfaces;

namespace Telemetria.Hardware.PowerModules.Interfaces
{
    public interface IParserPmModulesRPackage<out T> : IParserRPackage<T> where T : IRPackBase
    {

    }

    public interface IParserOuterPackPmModules
    {
        byte[] ParseData(byte[] data);
    }
}
