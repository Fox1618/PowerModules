using System;
using Hardware.DeviceSending.Interfaces;
using ProgrammingWeapons.Common;
using ProgrammingWeapons.Logging;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ReceivePackages;
using Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe;


namespace Telemetria.Hardware.PowerModules.ParsersRPack
{
    public class ParserTonheRPackage<T> : ParserPmModulesBase<T> where T: class, IRPackPmModulesBase
    {
        public ParserTonheRPackage(IParserOuterPackPmModules parserOuterPack) : base(parserOuterPack)
        {
        }

        public override T GetReceivePack(byte[] data, ISPackBase packS = null)
        {
            try
            {
                Log.Info("GetReceivePack: " + data.ToStrWoDashes());
                var dataInner = _ParserOuterPack.ParseData(data);
                if (dataInner == null) return null;
                return (T) getPack(dataInner, packS);
            }
            catch (Exception e)
            {
                Log.Warn(e.GetAllMessages());
                return null;
            }
        }

        private IRPackPmModulesBase getPack(byte[] data, ISPackBase packS = null)
        {
            Log.Info("CmdCode: " + data[1].ToString("x2"));
            var cmdCode = (CommandCodesTonhe)data[1];
            switch (cmdCode)
            {
                case CommandCodesTonhe.ChargingState:
                    return new RPackTonheModuleState(data);
                case CommandCodesTonhe.AcInfo:
                    return new RPackTonheAcInfo(data);
                case CommandCodesTonhe.ConfirmationStartStop:
                    return new RPackTonheConfirmStartStop(data);
            }
            return new RPackTonheBase(data);
        }
    }
}
