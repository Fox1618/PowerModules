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
                case (ushort)CommandCodesMegmeet.PowerControl:
                    return new RPackMegmeetPowerControl(data);
                case (ushort)CommandCodesMegmeet.VoltageSet:
                case (ushort)CommandCodesMegmeet.VoltageSetFloat:
                    return new RPackMegmeetSetVoltage(data);
                case (ushort)CommandCodesMegmeet.CurrentSet:
                case (ushort)CommandCodesMegmeet.CurrentSetFloat:
                    return new RPackMegmeetSetCurrent(data);
                case (ushort)CommandCodesMegmeet.ModeSet:
                    return new RPackMegmeetSetMode(data);
                case (ushort)CommandCodesMegmeet.StatusRead:
                    return new RPackMegmeetReadStatus(data);
                case (ushort)CommandCodesMegmeet.VoltageRead:
                    return new RPackMegmeetReadVoltage(data);
                case (ushort)CommandCodesMegmeet.CurrentRead:
                    return new RPackMegmeetReadCurrent(data);
                default:
                    return new RPackMegmeetBase(data);
            }
        }
    }
}
