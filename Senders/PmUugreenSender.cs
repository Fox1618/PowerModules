using System;
using System.Threading.Tasks;
using Hardware.DeviceSending;
using Hardware.DeviceSending.Interfaces;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen;
using Telemetria.Hardware.PowerModules.SendPackages.Uugreen;

namespace Telemetria.Hardware.PowerModules.Senders
{
    public class PmUugreenSender<T> : DeviceSendingWrapper<T>, IPmSender<T> where T : IRPackPmModulesBase
    {
        public PmUugreenSender(IDeviceSending deviceSending, IParserPmModulesRPackage<T> parserRPackage) : base(
            deviceSending, parserRPackage)
        {
        }

        public async Task<bool> PowerOn(byte? address = null)
        {
            if (address == null)
            {
                var sPackAll= new SPackUugreenPowerOnAll();
                var resAll = await SendPackageAsyncBool(sPackAll);
                return resAll.Result;
            }
            var sPackSpec = new SPackUugreenPowerOn(address.Value);
            var resSpec = await SendPackageAsync(sPackSpec);
            return resSpec != null;
        }

        public async Task<bool> ShutDown(byte? address = null)
        {
            if (address == null)
            {
                var sPackAll = new SPackUugreenShutdownAll();
                var resAll = await SendPackageAsyncBool(sPackAll);
                return resAll.Result;
            }
            var sPackSpec = new SPackUugreenShutdown(address.Value);
            var resSpec = await SendPackageAsync(sPackSpec);
            return resSpec != null;
        }

        public async Task<bool> SetVoltageAndCurrent(byte address, float voltage, float current)
        {
            var sPackVoltage = new SPackUugreenSetOutputVoltageRef(voltage, address);
            var resVoltage = await SendPackageAsyncBool(sPackVoltage);
            if (!resVoltage.Result || !(resVoltage.Pack is RPackUugreenSetCurrentLimit rSetOutputVoltage) ||
                Math.Abs(rSetOutputVoltage.Current - voltage) > 0.1) return false;

            var sPackCurrent = new SPackUugreenSetCurrentLimit(current, address);
            var resCurrent = await SendPackageAsyncBool(sPackCurrent);
            if (!resCurrent.Result || !(resCurrent.Pack is RPackUugreenSetCurrentLimit rSetCurrentLimit) ||
                Math.Abs(rSetCurrentLimit.Current - current) > 0.1) return false;
            return true;
        }

        public async Task<(float voltage, float current)?> GetVoltageCurrent(byte address)
        {
            var sPackVoltage = new SPackUugreenReadOutputVoltage(address);
            var resVoltage = await SendPackageAsyncBool(sPackVoltage);
            if (!resVoltage.Result || !(resVoltage.Pack is RPackUugreenSetCurrentLimit rGetOutputVoltage)) return null;

            var sPackCurrent = new SPackUugreenReadOutputCurrent(address);
            var resCurrent = await SendPackageAsyncBool(sPackCurrent);
            if (!resCurrent.Result || !(resCurrent.Pack is RPackUugreenSetCurrentLimit rGetCurrent)) return null;

            return (rGetOutputVoltage.Current, rGetCurrent.Current);
        }

        public async Task<(bool isValid, string errors, uint code)?> GetStatus(byte address)
        {
            var sPack = new SPackUugreenReadStatusFlags(address);
            var resStatus = await SendPackageAsyncBool(sPack);
            if (!resStatus.Result || !(resStatus.Pack is RPackUugreenReadStatusFlags rGetStatus)) return null;
            return (!rGetStatus.Flags.IsHaveFault, rGetStatus.Flags.Errors, rGetStatus.Flags.Code);
        }
    }
}