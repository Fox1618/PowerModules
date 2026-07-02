using System;
using Hardware.DeviceSending.Interfaces;
using ProgrammingWeapons.Common;
using ProgrammingWeapons.Logging;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ReceivePackages.Megmeet;
using Telemetria.Hardware.PowerModules.SendPackages.Megmeet;

namespace Telemetria.Hardware.PowerModules.ParsersRPack
{
    public class ParserMegmeetRPackage<T> : ParserPmModulesBase<T> where T : class, IRPackPmModulesBase
    {
        public ParserMegmeetRPackage(IParserOuterPackPmModules parserOuterPack) : base(parserOuterPack)
        {
        }

        public override T GetReceivePack(byte[] data, ISPackBase packS = null)
        {
            try
            {
                Log.Info("GetReceivePack: " + data.ToStrWoDashes());
                var dataInner = _ParserOuterPack.ParseData(data);
                if (dataInner == null) return null;
                return (T)getPack(dataInner, packS);
            }
            catch (Exception e)
            {
                Log.Warn(e.GetAllMessages());
                return null;
            }
        }

        private IRPackPmModulesBase getPack(byte[] data, ISPackBase packS = null)
        {
            var command = RPackMegmeetBase.ReadCommand(data);
            Log.Info("CmdCode: " + command.ToString("x4"));

            switch (command)
            {
                case MegmeetCommands.PowerControl:
                    return new RPackMegmeetPowerControl(data);
                case MegmeetCommands.VoltageSet:
                case MegmeetCommands.VoltageSetFloat:
                    return new RPackMegmeetSetVoltage(data);
                case MegmeetCommands.CurrentSet:
                case MegmeetCommands.CurrentSetFloat:
                    return new RPackMegmeetSetCurrent(data);
                case MegmeetCommands.ModeSet:
                    return new RPackMegmeetSetMode(data);
                case MegmeetCommands.StatusRead:
                    return new RPackMegmeetReadStatus(data);
                case MegmeetCommands.VoltageRead:
                    return new RPackMegmeetReadVoltage(data);
                case MegmeetCommands.CurrentRead:
                    return new RPackMegmeetReadCurrent(data);
                default:
                    return new RPackMegmeetBase(data);
            }
        }
    }
}
