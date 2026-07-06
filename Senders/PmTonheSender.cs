using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Hardware.DeviceSending;
using Hardware.DeviceSending.Interfaces;
using ProgrammingWeapons;
using ProgrammingWeapons.Logging;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe;
using Telemetria.Hardware.PowerModules.SendPackages.Tonhe;
using Telemetria.Hardware.TcpDevices;
using Timer = System.Timers.Timer;

namespace Telemetria.Hardware.PowerModules.Senders
{
    public sealed class TonhePmSender<T> : DeviceSendingWrapper<T>, IPmSender<T> where T : IRPackPmModulesBase
    {
        private readonly Timer _timerTiming;

        private ConcurrentDictionary<byte, (RPackTonheModuleState state, bool isWaiting, AutoResetEvent re)> _packStates;

        public TonhePmSender(IDeviceSending deviceSending, IParserPmModulesRPackage<T> parserRPackage) : base(
            deviceSending, parserRPackage, false)
        {
            _timerTiming = new Timer(5000){ AutoReset = true};
            _timerTiming.Elapsed += timerTimingElapsed;
            ReceivedPack += onReceivedPack;
            _packStates = new ConcurrentDictionary<byte, (RPackTonheModuleState state, bool isWaiting, AutoResetEvent re)>();
        }

        private void onReceivedPack(T packP)
        {
            if((int)packP.Status <= 1) return;
            Log.Info(JsonHelper.ReplaceBrackets(JsonHelper.SerializeObject(packP)));
            if (!_packStates.TryGetValue(packP.ModuleAddress, out var value) || !value.isWaiting) return;
            var pack = (IRPackPmModulesBase) packP;
            if (pack is RPackTonheModuleState packState)
            {
                _packStates[packP.ModuleAddress] = (packState, false, value.re);
                value.re.Set();
            }
        }

        private void timerTimingElapsed(object sender, ElapsedEventArgs e)
        {
            var timingPack = new SPackTonheTiming();
            _ = SendPackageAsyncBool(timingPack);
        }

        public async Task<bool> PowerOnOffAll(bool isStart)
        {
            if (isStart)
            {
                var sPackAll = new SPackTonheStartStop { IsStart = true };
                var resAll = await SendPackageAsyncBool(sPackAll);
                return resAll.Result;
            }
            else
            {
                var sPackAll = new SPackTonheStartStop{ IsStart = false};
                var resAll = await SendPackageAsyncBool(sPackAll);
                return resAll.Result;
            }
        }

        public async Task<bool> PowerOnOffSpecModule(bool isStart, byte address, float? voltage, float? current)
        {
            if (isStart)
            {
                if (voltage != null && current != null)
                {
                    var sPackSpec = new SPackTonheStartStopSpecModule(address, voltage.Value, current.Value) { IsStart = true };
                    var resSpec = await SendPackageAsyncBool(sPackSpec);
                    return resSpec.Result;
                }
                var sPackAll = new SPackTonheStartStop (address) { IsStart = true };
                var resAll = await SendPackageAsyncBool(sPackAll);
                return resAll.Result;
            }
            else
            {
                var sPackSpec = new SPackTonheStartStopSpecModule(address, 0, 0) { IsStart = false };
                var resSpec = await SendPackageAsyncBool(sPackSpec);
                return resSpec.Result;
                /*var sPackAll = new SPackTonheStartStop (address) { IsStart = false};
                var resAll = await SendPackageAsyncBool(sPackAll);
                return resAll.Result;*/
            }
        }

        public async Task<bool> SetVoltageAndCurrent(byte address, float voltage, float current)
        {
            var sPackSpec = new SPackTonheParamSetting(address, voltage, current);
            var resSpec = await SendPackageAsyncBool(sPackSpec);
            return resSpec.Result;
        }

        public async Task<(float voltage, float current)?> GetVoltageCurrent(byte address)
        {
            var packState = await getCurrentState(address);
            if (packState == null) return null;
            return (packState.State.Voltage, packState.State.Current);
        }

        public async Task<StateBase> GetStatus(byte address)
        {
            var packState = await getCurrentState(address);
            return packState.State.StateBits;
        }

        public void Start()
        {
            if (DeviceSending is TcpDevice tcpDevice)
            {
                Log.Info($"Tonhe: Попытка подключения к TCP устройству...");
                if (tcpDevice.Connect())
                {
                    Log.Info($"Tonhe: TCP подключение успешно установлено");
                    tcpDevice.StartListen();
                    timerTimingElapsed(null, null);
                    _timerTiming.Start();
                }
                else
                {
                    Log.Error($"Tonhe: Не удалось подключиться к TCP устройству. Проверьте IP адрес и порт.");
                }
            }
            else
            {
                Log.Error($"Tonhe: DeviceSending не является TcpDevice");
            }
        }

        public void Stop()
        {
            _timerTiming.Stop();
            if (DeviceSending is TcpDevice tcpDevice) {
                tcpDevice.StopListen();
            }
        }

        public async Task<PmInfo> GetVoltageCurrentAndStatus(byte address)
        {
            var packState = await getCurrentState(address);
            if (packState == null) return null;
            return new PmInfo {Voltage = packState.State.Voltage, Current = packState.State.Current, State = packState.State.StateBits};
        }

        private async Task<RPackTonheModuleState> getCurrentState(byte address)
        {
          return await Task.Run(() =>
          {
              var isExist = _packStates.TryGetValue(address, out var value);
              if (isExist)
                  value.re.WaitOne(3000);
              var r = _packStates.AddOrUpdate(address, b => (null, true, new AutoResetEvent(false)),
                  (b, tuple) => (null, true, tuple.re));
              var resWait = r.re.WaitOne(3000);
              var state = resWait? _packStates[address].state : null;
              r.re.Set();
              return state;
          });
        }
    }
}