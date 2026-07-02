using System.Threading.Tasks;
using System.Timers;
using Hardware.DeviceSending;
using Hardware.DeviceSending.Interfaces;
using ProgrammingWeapons;
using ProgrammingWeapons.Logging;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.SendPackages.Megmeet;
using Telemetria.Hardware.TcpDevices;
using Timer = System.Timers.Timer;

namespace Telemetria.Hardware.PowerModules.Senders
{
    public sealed class MegmeetPmSender<T> : DeviceSendingWrapper<T>, IPmSender<T> where T : IRPackPmModulesBase
    {
        private readonly Timer _timerTiming;

        public MegmeetPmSender(IDeviceSending deviceSending, IParserPmModulesRPackage<T> parserRPackage) : base(
            deviceSending, parserRPackage, false)
        {
            _timerTiming = new Timer(5000) { AutoReset = true };
            _timerTiming.Elapsed += timerTimingElapsed;
            deviceSending.IsWriteLog = false;
            ReceivedPack += onReceivedPack;
        }

        private void timerTimingElapsed(object sender, ElapsedEventArgs e)
        {
            var timingPack = new SPackMegmeetReadStatus(0xFF);
            _ = SendPackageAsyncBool(timingPack);
        }

        private void onReceivedPack(T packP)
        {
            if ((int)packP.Status <= 1) return;
            Log.Info(JsonHelper.ReplaceBrackets(JsonHelper.SerializeObject(packP)));
        }

        public async Task<bool> PowerOnOffAll(bool isStart)
        {
            var sPack = new SPackMegmeetPowerControl(0xFF, isStart);
            var res = await SendPackageAsyncBool(sPack);
            return res.Result;
        }

        public async Task<bool> PowerOnOffSpecModule(bool isStart, byte address, float? voltage, float? current)
        {
            if (isStart)
            {
                if (voltage.HasValue)
                {
                    var modePack = voltage.Value > 500
                        ? new SPackMegmeetSetMode(address, MegmeetMode.High)
                        : new SPackMegmeetSetMode(address, MegmeetMode.Low);
                    var modeRes = await SendPackageAsyncBool(modePack);
                    if (!modeRes.Result) return false;
                }

                if (voltage.HasValue && current.HasValue)
                {
                    var setRes = await SetVoltageAndCurrent(address, voltage.Value, current.Value);
                    if (!setRes) return false;
                }

                await Task.Delay(300);
            }

            var sPack = new SPackMegmeetPowerControl(address, isStart);
            var res = await SendPackageAsyncBool(sPack);
            return res.Result;
        }

        public async Task<bool> SetVoltageAndCurrent(byte address, float voltage, float current)
        {
            var voltagePack = new SPackMegmeetSetVoltage(address, voltage);
            var voltageRes = await SendPackageAsyncBool(voltagePack);
            if (!voltageRes.Result) return false;

            var currentPack = new SPackMegmeetSetCurrent(address, current);
            var currentRes = await SendPackageAsyncBool(currentPack);
            return currentRes.Result;
        }

        public async Task<(float voltage, float current)?> GetVoltageCurrent(byte address)
        {
            var voltagePack = new SPackMegmeetReadVoltage(address);
            var voltageRes = await SendPackageAsyncBool(voltagePack);
            if (!voltageRes.Result || !(voltageRes.Pack is T rVoltage)) return null;

            var currentPack = new SPackMegmeetReadCurrent(address);
            var currentRes = await SendPackageAsyncBool(currentPack);
            if (!currentRes.Result || !(currentRes.Pack is T rCurrent)) return null;

            return (readValue(rVoltage), readValue(rCurrent));
        }

        public async Task<StateBase> GetStatus(byte address)
        {
            var statusPack = new SPackMegmeetReadStatus(address);
            var statusRes = await SendPackageAsyncBool(statusPack);
            if (!statusRes.Result || !(statusRes.Pack is T rStatus) || rStatus.Data == null || rStatus.Data.Length < 4) return null;
            return new StateBase(rStatus.Data);
        }

        public void Start()
        {
            if (DeviceSending is TcpDevice tcpDevice)
            {
                if (tcpDevice.Connect())
                {
                    tcpDevice.StartListen();
                    timerTimingElapsed(null, null);
                    _timerTiming.Start();
                }
            }
        }

        public void Stop()
        {
            _timerTiming.Stop();
            if (DeviceSending is TcpDevice tcpDevice)
            {
                tcpDevice.StopListen();
            }
        }

        public async Task<PmInfo> GetVoltageCurrentAndStatus(byte address)
        {
            var voltCurrent = await GetVoltageCurrent(address);
            if (voltCurrent == null) return null;
            var state = await GetStatus(address);
            return new PmInfo { Voltage = voltCurrent.Value.voltage, Current = voltCurrent.Value.current, State = state };
        }

        private float readValue(T pack)
        {
            if (pack.Data == null || pack.Data.Length < 8) return 0;
            return readUInt32(pack.Data, 4) * 0.001f;
        }

        private uint readUInt32(byte[] data, int startIndex)
        {
            return (uint)(data[startIndex] << 24 |
                          data[startIndex + 1] << 16 |
                          data[startIndex + 2] << 8 |
                          data[startIndex + 3]);
        }
    }
}
