using Hardware.DeviceSending.Interfaces;
using ProgrammingWeapons.Common;
using ProgrammingWeapons.Logging;
using System;
using System.Diagnostics;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ReceivePackages.Sinexcel;
using Telemetria.Hardware.PowerModules.SendPackages.Sinexcel;

namespace Telemetria.Hardware.PowerModules.ParsersRPack
{
    public class ParserSinexcelRPackage<T> : ParserPmModulesBase<T> where T : class, IRPackPmModulesBase
    {
        public ParserSinexcelRPackage(IParserOuterPackPmModules parserOuterPack) : base(parserOuterPack)
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
            if (data == null || data.Length < 4)
            {
                Debug.WriteLine($"[Sinexcel] data in package is null or <4. (Parser)");
                Log.Warn("Sinexcel parser: Inner data is null or too short to read Command ID.");
                return new RPackSinexcelBase(data ?? new byte[0]);
            }
            ushort command = (ushort)((data[2] << 8) | data[3]);
            Log.Info("CmdCode: " + command.ToString("x4"));

            switch (command)
            {
                case (ushort)CommandCodesSinexcel.QueryOutputVoltage:
                    return new RPackSinexcelReadVoltage(data);

                case (ushort)CommandCodesSinexcel.QueryOutputCurrent:
                    return new RPackSinexcelReadCurrent(data);

                case (ushort)CommandCodesSinexcel.QueryDcStatusFault:
                case (ushort)CommandCodesSinexcel.QueryAcStatusFault:
                case (ushort)CommandCodesSinexcel.QueryDcOperatingStatus:
                    return new RPackSinexcelReadStatus(data);

                case (ushort)CommandCodesSinexcel.SetOutputVoltage:
                    return new RPackSinexcelSetVoltage(data);

                case (ushort)CommandCodesSinexcel.SetOutputCurrent:
                    return new RPackSinexcelSetCurrent(data);

                case (ushort)CommandCodesSinexcel.PowerControlLow:
                case (ushort)CommandCodesSinexcel.PowerControlHigh:
                    return new RPackSinexcelPowerControl(data);

                case (ushort)CommandCodesSinexcel.SetVoltageMode:
                    return new RPackSinexcelSetMode(data);

                default:
                    return new RPackSinexcelBase(data);
            }
        }
    }
}