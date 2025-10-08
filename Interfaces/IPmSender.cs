using Hardware.DeviceSending.Interfaces;

namespace Telemetria.Hardware.PowerModules.Interfaces
{
    public interface IPmSender<T> : IDeviceSendingWrapper<T>, INotifier where T : IRPackBase
    {

    }
}