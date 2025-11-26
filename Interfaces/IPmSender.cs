using System.Threading.Tasks;
using Hardware.DeviceSending.Interfaces;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.Interfaces
{
    public interface IPmSender<T> : IDeviceSendingWrapper<T>, INotifier where T : IRPackBase
    {
        Task<bool> PowerOnOffAll(bool isStart);
        Task<bool> PowerOnOffSpecModule(bool isStart, byte address, float? voltage, float? current);
        Task<bool> SetVoltageAndCurrent(byte address, float voltage, float current);
        Task<(float voltage, float current)?> GetVoltageCurrent(byte address);
        Task<StateBase> GetStatus(byte address);
        void Start();
        void Stop();
        Task<PmInfo> GetVoltageCurrentAndStatus(byte address);
    }
}