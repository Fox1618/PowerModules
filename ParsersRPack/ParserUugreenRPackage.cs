using System;
using System.Linq;
using Hardware.DeviceSending.Interfaces;
using ProgrammingWeapons.Common;
using ProgrammingWeapons.Logging;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ReceivePackages;
using Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe;
using Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen;


namespace Telemetria.Hardware.PowerModules.ParsersRPack
{
    public class ParserUugreenRPackage<T> : ParserPmModulesBase<T> where T: class, IRPackPmModulesBase
    {
        public ParserUugreenRPackage(IParserOuterPackPmModules parserOuterPack) : base(parserOuterPack)
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

            var cmdType = (CommandCodesUugreen)RPackUugreenBase.GetCommandType(data);
            var messageType = (MessageTypeUugreen)RPackUugreenBase.GetMessageType(data);

            switch (messageType)
            {
                case MessageTypeUugreen.SetDataResponse:
                    switch (cmdType)
                    {
                        case CommandCodesUugreen.VOutReference:
                            return new RPackUugreenSetOutputVoltageRef(data);
                        case CommandCodesUugreen.IOutLimit:
                            return new RPackUugreenSetCurrentLimit(data);
                        case CommandCodesUugreen.ShutDownDcDc:
                            if (RPackUugreenBase.GetData(data).All(x => x == 0)) {
                                return new RPackUugreenPowerOn(data);
                            }
                            return new RPackUugreenShutdown(data);
                        default: return new RPackUugreenBase(data);
                    }
                case MessageTypeUugreen.ReadDataResponse:
                    switch (cmdType)
                    {
                        case CommandCodesUugreen.Vout:
                            return new RPackUugreenReadOutputVoltage(data);
                        case CommandCodesUugreen.IOutSlow:
                            return new RPackUugreenReadOutputCurrent(data);
                        case CommandCodesUugreen.ModuleStatus:
                            return new RPackUugreenReadStatusFlags(data);
                        default: return new RPackUugreenBase(data);
                    }
                case MessageTypeUugreen.AllSetDataResponse:
                    if (cmdType == CommandCodesUugreen.Vout)
                        return new RPackUugreenAllSetData(data);
                    break;
            }
            return new RPackUugreenBase(data);
        }
    }
}
