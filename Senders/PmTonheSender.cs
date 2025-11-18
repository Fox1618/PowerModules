using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Hardware.DeviceSending;
using Hardware.DeviceSending.Interfaces;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe;
using Telemetria.Hardware.PowerModules.SendPackages.Tonhe;
using Timer = System.Timers.Timer;

namespace Telemetria.Hardware.PowerModules.Senders
{
    public sealed class TonhePmSender<T> : DeviceSendingWrapper<T>, IPmSender<T> where T : IRPackPmModulesBase
    {
        private readonly Timer _timerTiming;

        private ConcurrentDictionary<byte, (RPackTonheModuleState state, bool isWaiting, AutoResetEvent re)> _packStates;

        public TonhePmSender(IDeviceSending deviceSending, IParserPmModulesRPackage<T> parserRPackage) : base(
            deviceSending, parserRPackage)
        {
            _timerTiming = new Timer(5000){ AutoReset = true};
            _timerTiming.Elapsed += timerTimingElapsed;
            ReceivedPack += onReceivedPack;
            _packStates = new ConcurrentDictionary<byte, (RPackTonheModuleState state, bool isWaiting, AutoResetEvent re)>();
        }

        private void onReceivedPack(T packP)
        {
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
                    var resSpec = await SendPackageAsync(sPackSpec);
                    return resSpec != null;
                }
                var sPackAll = new SPackTonheStartStop (address) { IsStart = true };
                var resAll = await SendPackageAsyncBool(sPackAll);
                return resAll.Result;
            }
            else
            {
                var sPackAll = new SPackTonheStartStop (address) { IsStart = false };
                var resAll = await SendPackageAsyncBool(sPackAll);
                return resAll.Result;
            }
        }

        public async Task<bool> SetVoltageAndCurrent(byte address, float voltage, float current)
        {
            var sPackSpec = new SPackTonheParamSetting(address, voltage, current);
            var resSpec = await SendPackageAsync(sPackSpec);
            return resSpec != null;
        }

        public async Task<(float voltage, float current)?> GetVoltageCurrent(byte address)
        {
            var packState = await getCurrentState(address);
            return (packState.State.Voltage, packState.State.Current);
        }

        public async Task<StateBase> GetStatus(byte address)
        {
            var packState = await getCurrentState(address);
            return packState.State.StateBits;
        }

        public void Start()
        {
            _timerTiming.Start();
        }

        public void Stop()
        {
            _timerTiming.Stop();
        }

        public async Task<(float voltage, float current, StateBase)?> GetVoltageCurrentAndStatus(byte address)
        {
            var packState = await getCurrentState(address);
            if (packState == null) return null;
            return (packState.State.Voltage, packState.State.Current, packState.State.StateBits);
        }

        private async Task<RPackTonheModuleState> getCurrentState(byte address)
        {
          return await Task.Run(() =>
          {
              var r = _packStates.AddOrUpdate(address, b => (null, true, new AutoResetEvent(true)),
                  (b, tuple) => (null, true, tuple.re));
              r.re.WaitOne(3000);
              return _packStates[address].state;
          });
        }
    }
}