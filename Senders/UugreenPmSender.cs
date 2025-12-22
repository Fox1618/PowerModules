using System;
using System.Threading.Tasks;
using Hardware.DeviceSending;
using Hardware.DeviceSending.Interfaces;
using ProgrammingWeapons.Logging;
using ProgrammingWeapons;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen;
using Telemetria.Hardware.PowerModules.SendPackages.Uugreen;
using Telemetria.Hardware.TcpDevices;
using System.Timers;
using Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen.Models;

namespace Telemetria.Hardware.PowerModules.Senders
{
    public sealed class UugreenPmSender<T> : DeviceSendingWrapper<T>, IPmSender<T> where T : IRPackPmModulesBase
    {
        private readonly Timer _timerTiming;

        public UugreenPmSender(IDeviceSending deviceSending, IParserPmModulesRPackage<T> parserRPackage) : base(
            deviceSending, parserRPackage, false)
        {
            _timerTiming = new Timer(5000) { AutoReset = true };
            _timerTiming.Elapsed += timerTimingElapsed;
            deviceSending.IsWriteLog = false;
            ReceivedPack += OnReceivedPack;
        }
        private void timerTimingElapsed(object sender, ElapsedEventArgs e)
        {
            var timingPack = new SPackUugreenReadStatusFlags(0);
            _ = SendPackageAsyncBool(timingPack);
        }
        private void OnReceivedPack(T packP)
        {
            if ((int)packP.Status <= 1) return;
            Log.Info(JsonHelper.ReplaceBrackets(JsonHelper.SerializeObject(packP)));
        }

        private async Task<bool> setMode(byte address, HiLoModeUugreen mode)
        {
            var packGetMode = new SPackUugreenGetLowOrHighVoltageMode(address);
            var resGetMode = await SendPackageAsyncBool(packGetMode);
            if (!resGetMode.Result) return false;
            if (resGetMode.Pack is RPackUugreenGetLowOrHighVoltageMode rPackGetMode &&
                rPackGetMode.ModeEnum == mode)
                return true;
            var sPackSpec = new SPackUugreenShutdown(address);
            var resSpec = await SendPackageAsync(sPackSpec);
            if (resSpec != null)
                await Task.Delay(500);
            else {
                return false;
            }
            var packSetMode = new SPackUugreenSetLowOrHighVoltageMode(address, mode);
            var resSetMode = await SendPackageAsyncBool(packSetMode);
            if (!resSetMode.Result) return false;
            if (resSetMode.Pack is RPackUugreenSetLowOrHighVoltageMode rPackSetMode &&
                rPackSetMode.ModeEnum == mode) {
                return true;
            }
            return false;
        }
        public async Task<bool> PowerOnOffAll(bool isStart)
        {
            if (isStart)
            {
                var sPackAll = new SPackUugreenPowerOnAll();
                var resAll = await SendPackageAsyncBool(sPackAll);
                return resAll.Result;
            }
            else
            {
                var sPackAll = new SPackUugreenShutdownAll();
                var resAll = await SendPackageAsyncBool(sPackAll);
                return resAll.Result;
            }
        }

        /*public async Task<bool> PowerOnOffSpecModule(bool isStart, byte address, float? voltage = null, float? current = null)
        {
            if (isStart)
            {
                if (voltage != null && current != null)
                {
                    var mode = voltage > 500 ? HiLoModeUugreen.HiModeSingleCmd : HiLoModeUugreen.NoSelection;
                    var resSetMode = await setMode(address, mode);
                    if(!resSetMode) return false;
                    var resSet = await SetVoltageAndCurrent(address, voltage.Value, current.Value);
                    if (!resSet) return false;
                }
                await Task.Delay(300);
                var sPackSpec = new SPackUugreenPowerOn(address);
                var resSpec = await SendPackageAsync(sPackSpec);
                return resSpec != null;
            }
            else
            {
                var sPackSpec = new SPackUugreenShutdown(address);
                var resSpec = await SendPackageAsync(sPackSpec);
                return resSpec != null;
            }
        }*/
        public async Task<bool> PowerOnOffSpecModule(bool isStart, byte address, float? voltage = null, float? current = null)
        {
            var voltageP = voltage ?? 0f;
            var currentP = current ?? 0f;
            var hiLowMode = voltage > 500 ? HiLoModeUugreen.HiMode : HiLoModeUugreen.Low;
            var onOffMode = isStart ? OnOffModeUugreen.TurnOnDcPfc: OnOffModeUugreen.ShutDownDcPfc;
            var mode = new HiLoModeOnOffUugreen { HiLoMode = hiLowMode, OnOffMode = onOffMode};
            var sPackSpec = new SPackUugreenAllSetData(address, voltageP, voltageP, currentP, mode);
            var resSpec = await SendPackageAsyncBool(sPackSpec);
            if (!resSpec.Result || !(resSpec.Pack is RPackUugreenAllSetData rSetAllData) ||
                Math.Abs(rSetAllData.Current - currentP) > 0.1 || rSetAllData.Voltage - voltageP > 0.1) return false;
            return true;
        }
        public async Task<bool> SetVoltageAndCurrent(byte address, float voltage, float current)
        {
            var sPackCurrent = new SPackUugreenSetCurrentLimit(current, address);
            var resCurrent = await SendPackageAsyncBool(sPackCurrent);
            if (!resCurrent.Result || !(resCurrent.Pack is RPackUugreenSetCurrentLimit rSetCurrentLimit) ||
                Math.Abs(rSetCurrentLimit.Current - current) > 0.1) return false;

            var sPackVoltage = new SPackUugreenSetOutputVoltageRef(voltage, address);
            var resVoltage = await SendPackageAsyncBool(sPackVoltage);
            if (!resVoltage.Result || !(resVoltage.Pack is RPackUugreenSetOutputVoltageRef rSetOutputVoltage) ||
                Math.Abs(rSetOutputVoltage.Voltage - voltage) > 0.1) return false;
            return true;
        }

        public async Task<(float voltage, float current)?> GetVoltageCurrent(byte address)
        {
            var sPackVoltage = new SPackUugreenReadOutputVoltage(address);
            var resVoltage = await SendPackageAsyncBool(sPackVoltage);
            if (!resVoltage.Result || !(resVoltage.Pack is RPackUugreenReadOutputVoltage rGetOutputVoltage)) return null;

            var sPackCurrent = new SPackUugreenReadOutputCurrent(address);
            var resCurrent = await SendPackageAsyncBool(sPackCurrent);
            if (!resCurrent.Result || !(resCurrent.Pack is RPackUugreenReadOutputCurrent rGetCurrent)) return null;

            return (rGetOutputVoltage.Voltage, rGetCurrent.Current);
        }

        public async Task<StateBase> GetStatus(byte address)
        {
            var sPack = new SPackUugreenReadStatusFlags(address);
            var resStatus = await SendPackageAsyncBool(sPack);
            if (!resStatus.Result || !(resStatus.Pack is RPackUugreenReadStatusFlags rGetStatus)) return null;
            return rGetStatus.Flags;
        }

        public void Start()
        {
            if (DeviceSending is TcpDevice tcpDevice) {
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
            if (DeviceSending is TcpDevice tcpDevice) {
                tcpDevice.StopListen();
            }
        }

        public async Task<PmInfo> GetVoltageCurrentAndStatus(byte address)
        {
            var voltCurrent = await GetVoltageCurrent(address);
            if (voltCurrent == null) return null;
            var state = await GetStatus(address);
            return new PmInfo{Voltage = voltCurrent.Value.voltage, Current = voltCurrent.Value.current, State = state};
        }
    }
}