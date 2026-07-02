using System.Threading.Tasks;
using Hardware.DeviceSending;
using Hardware.DeviceSending.Interfaces;
using ProgrammingWeapons;
using ProgrammingWeapons.Logging;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.TcpDevices;

namespace Telemetria.Hardware.PowerModules.Senders
{
    public sealed class MegmeetPmSender<T> : DeviceSendingWrapper<T>, IPmSender<T> where T : IRPackPmModulesBase
    {
        public MegmeetPmSender(IDeviceSending deviceSending, IParserPmModulesRPackage<T> parserRPackage) : base(
            deviceSending, parserRPackage, false)
        {
            deviceSending.IsWriteLog = false;
            ReceivedPack += onReceivedPack;
        }

        private void onReceivedPack(T packP)
        {
            if ((int)packP.Status <= 1) return;
            Log.Info(JsonHelper.ReplaceBrackets(JsonHelper.SerializeObject(packP)));
        }

        public Task<bool> PowerOnOffAll(bool isStart)
        {
            return Task.FromResult(false);
        }

        public Task<bool> PowerOnOffSpecModule(bool isStart, byte address, float? voltage, float? current)
        {
            return Task.FromResult(false);
        }

        public Task<bool> SetVoltageAndCurrent(byte address, float voltage, float current)
        {
            return Task.FromResult(false);
        }

        public Task<(float voltage, float current)?> GetVoltageCurrent(byte address)
        {
            return Task.FromResult<(float voltage, float current)?>(null);
        }

        public Task<StateBase> GetStatus(byte address)
        {
            return Task.FromResult<StateBase>(null);
        }

        public void Start()
        {
            if (DeviceSending is TcpDevice tcpDevice)
            {
                if (tcpDevice.Connect())
                {
                    tcpDevice.StartListen();
                }
            }
        }

        public void Stop()
        {
            if (DeviceSending is TcpDevice tcpDevice)
            {
                tcpDevice.StopListen();
            }
        }

        public Task<PmInfo> GetVoltageCurrentAndStatus(byte address)
        {
            return Task.FromResult<PmInfo>(null);
        }
    }
}
