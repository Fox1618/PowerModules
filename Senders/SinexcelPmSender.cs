using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Hardware.DeviceSending;
using Hardware.DeviceSending.Interfaces;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ReceivePackages.Sinexcel;
using Telemetria.Hardware.PowerModules.SendPackages.Sinexcel;
using Telemetria.Hardware.TcpDevices;
using Timer = System.Timers.Timer;

namespace Telemetria.Hardware.PowerModules.Senders
{
    public sealed class SinexcelPmSender<T> : DeviceSendingWrapper<T>, IPmSender<T> where T : IRPackPmModulesBase
    {
        private readonly Timer _timerTiming;

        private byte _activeGroup = 0;
        private byte _activeSubAddress = 1;

        public SinexcelPmSender(IDeviceSending deviceSending, IParserPmModulesRPackage<T> parserRPackage) : base(
            deviceSending, parserRPackage, false)
        {
            _timerTiming = new Timer(5000) { AutoReset = true };
            _timerTiming.Elapsed += timerTimingElapsed;
            deviceSending.IsWriteLog = false;
            ReceivedPack += onReceivedPack;
        }

        private void timerTimingElapsed(object sender, ElapsedEventArgs e)
        {
            var timingPack = new SPackSinexcelReadStatus(_activeGroup, _activeSubAddress);
            _ = SendPackageAsyncBool(timingPack);
        }

        private void onReceivedPack(T packP)
        {
            if ((int)packP.Status <= 1) return;

            if (packP is RPackSinexcelBase sinexcelPack)
            {
                _activeGroup = sinexcelPack.Group;
                _activeSubAddress = sinexcelPack.SubAddress;
            }
        }

        public async Task<bool> PowerOnOffAll(bool isStart)
        {
            var sPack = new SPackSinexcelPowerControl(31, 0, isStart);
            var res = await SendPackageAsyncBool(sPack);
            return res.Result;
        }

        public async Task<bool> PowerOnOffSpecModule(bool isStart, byte address, float? voltage, float? current)
        {
            byte group = (byte)(address >> 6);
            byte subAddress = (byte)(address & 0x3F);

            if (isStart)
            {
                if (voltage.HasValue)
                {
                    SendPackages.Sinexcel.SinexcelMode mode = voltage.Value > 500
                        ? SendPackages.Sinexcel.SinexcelMode.High
                        : SendPackages.Sinexcel.SinexcelMode.Low;

                    var modePack = new SPackSinexcelSetMode(group, subAddress, mode);
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

            var sPack = new SPackSinexcelPowerControl(group, subAddress, isStart);
            var res = await SendPackageAsyncBool(sPack);
            return res.Result;
        }

        public async Task<bool> SetVoltageAndCurrent(byte address, float voltage, float current)
        {
            byte group = (byte)(address >> 6);
            byte subAddress = (byte)(address & 0x3F);

            var voltPack = new SPackSinexcelSetVoltage(group, subAddress, voltage);
            var currPack = new SPackSinexcelSetCurrent(group, subAddress, current);

            var resV = await SendPackageAsyncBool(voltPack);
            var resC = await SendPackageAsyncBool(currPack);

            return resV.Result && resC.Result;
        }

        public async Task<(float voltage, float current)?> GetVoltageCurrent(byte address)
        {
            byte group = (byte)(address >> 6);
            byte subAddress = (byte)(address & 0x3F);

            var queryPack = new SPackSinexcelReadStatus(group, subAddress);
            var queryRes = await SendPackageAsyncBool(queryPack);

            if (!queryRes.Result || !(queryRes.Pack is RPackSinexcelReadStatus rQuery))
            {
                return null;
            }

            return (rQuery.VoltageValue, rQuery.CurrentValue);
        }

        public async Task<StateBase> GetStatus(byte address)
        {
            byte group = (byte)(address >> 6);
            byte subAddress = (byte)(address & 0x3F);

            var queryPack = new SPackSinexcelReadStatus(group, subAddress);
            var queryRes = await SendPackageAsyncBool(queryPack);

            if (!queryRes.Result || !(queryRes.Pack is RPackSinexcelReadStatus rQuery))
            {
                return null;
            }

            var state = new StateBase(rQuery.Data);

            return state;
        }

        public void Start()
        {
            if (DeviceSending is TcpDevice tcpDevice)
            {
                if (tcpDevice.Connect())
                {
                    Debug.WriteLine($"[Sinexcel] TCP подключение успешно установлено");
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
            byte group = (byte)(address >> 6);
            byte subAddress = (byte)(address & 0x3F);

            var queryPack = new SPackSinexcelReadStatus(group, subAddress);
            var queryRes = await SendPackageAsyncBool(queryPack);

            if (!queryRes.Result || !(queryRes.Pack is RPackSinexcelReadStatus rQuery))
            {
                return null;
            }

            var state = new StateBase(rQuery.Data);

            return new PmInfo
            {
                Voltage = rQuery.VoltageValue,
                Current = rQuery.CurrentValue,
                State = state
            };
        }
    }
}