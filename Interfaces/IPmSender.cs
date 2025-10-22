using System.Threading.Tasks;
using Hardware.DeviceSending.Interfaces;

namespace Telemetria.Hardware.PowerModules.Interfaces
{
    public interface IPmSender<T> : IDeviceSendingWrapper<T>, INotifier where T : IRPackBase
    {
        Task<bool> PowerOn(byte? address = null);
        Task<bool> ShutDown(byte? address = null);
        Task<bool> SetVoltageAndCurrent(byte address, float voltage, float current);
        Task<(float voltage, float current)?> GetVoltageCurrent(byte address);
        Task<(bool isValid, string errors, uint code)?> GetStatus(byte address);
    }
}