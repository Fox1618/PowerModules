using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Hardware.DeviceSending;
using Hardware.DeviceSending.Interfaces;
using ProgrammingWeapons;
using ProgrammingWeapons.Logging;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ReceivePackages.Megmeet;
using Telemetria.Hardware.PowerModules.SendPackages.Megmeet;
using Telemetria.Hardware.TcpDevices;
using Timer = System.Timers.Timer;

namespace Telemetria.Hardware.PowerModules.Senders
{
    public sealed class MegmeetPmSender<T> : DeviceSendingWrapper<T>, IPmSender<T> where T : IRPackPmModulesBase
    {
        private readonly Timer _timerTiming;

        private byte _activeAddress = 1;
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
            var timingPack = new SPackMegmeetReadStatus(_activeAddress);
            _ = SendPackageAsyncBool(timingPack);
        }

        private void onReceivedPack(T packP)
        {
            if ((int)packP.Status <= 1) return;
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

                    var modeRes = await SendPackageAsyncBool((ISPackBase)modePack);

                    if (!modeRes.Result || !(modeRes.Pack is RPackMegmeetSetMode rSetMode) || rSetMode.Mode != modePack.Mode)
                    {
                        return false;
                    }
                }

                if (voltage.HasValue && current.HasValue)
                {
                    var setRes = await SetVoltageAndCurrent(address, voltage.Value, current.Value);
                    if (!setRes)
                    {
                        return false;
                    }
                }

                await Task.Delay(300);
            }

            var sPack = new SPackMegmeetPowerControl(address, isStart);
            var res = await SendPackageAsyncBool(sPack);
            var result = res.Result && res.Pack is RPackMegmeetPowerControl rPowerControl && rPowerControl.IsStart == isStart;
            return result;
        }

        public async Task<bool> SetVoltageAndCurrent(byte address, float voltage, float current)
        {
            _activeAddress = address; 
            var voltPack = new MegmeetFloatPack(address, (ushort)CommandCodesMegmeet.VoltageSetFloat, voltage);
            var currPack = new MegmeetFloatPack(address, (ushort)CommandCodesMegmeet.CurrentSetFloat, current);

            var resV = await SendPackageAsyncBool((ISPackBase)voltPack);
            var resC = await SendPackageAsyncBool((ISPackBase)currPack);

            return resV.Result && resC.Result;
        }
        public async Task<(float voltage, float current)?> GetVoltageCurrent(byte address)
        {
            var voltagePack = new SPackMegmeetReadVoltage(address);
            var voltageRes = await SendPackageAsyncBool(voltagePack);
            if (!voltageRes.Result || !(voltageRes.Pack is RPackMegmeetReadVoltage rVoltage))
            {
                return null;
            }

            var currentPack = new SPackMegmeetReadCurrent(address);
            var currentRes = await SendPackageAsyncBool(currentPack);
            if (!currentRes.Result || !(currentRes.Pack is RPackMegmeetReadCurrent rCurrent))
            {
                return null;
            }

            return (rVoltage.Voltage, rCurrent.Current);
        }

        public async Task<StateBase> GetStatus(byte address)
        {
            var statusPack = new SPackMegmeetReadStatus(address);
            var statusRes = await SendPackageAsyncBool(statusPack);
            if (!statusRes.Result || !(statusRes.Pack is RPackMegmeetReadStatus rStatus))
            {
                return null;
            }
            return rStatus.State;
        }

        public void Start()
        {
            if (DeviceSending is TcpDevice tcpDevice)
            {
                if (tcpDevice.Connect())
                {
                    Debug.WriteLine($"[Megmeet] TCP подключение успешно установлено");
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
            _activeAddress = address;
            
            var voltCurrent = await GetVoltageCurrent(address);
            if (voltCurrent == null)
            {
                return null;
            }
            
            var state = await GetStatus(address);
            if (state == null)
            {
                return null;
            }
            
            return new PmInfo { Voltage = voltCurrent.Value.voltage, Current = voltCurrent.Value.current, State = state };
        }

    }
    internal class MegmeetFloatPack : SPackMegmeetBase
    {
        public MegmeetFloatPack(byte moduleAddress, ushort command, float value)
            : base(moduleAddress, command)
        {
            SetFloatValue(value);
        }
    }
    internal class MegmeetUintPack : SPackMegmeetBase
    {
        public MegmeetUintPack(byte moduleAddress, ushort command, uint value)
            : base(moduleAddress, command)
        {
            SetUInt32Value(value);
        }
    }
}
