using Telemetria.Hardware.PowerModules.Interfaces;

namespace Telemetria.Hardware.PowerModules.ParsersRPack
{
    public class ParserOuterPackPmModules : IParserOuterPackPmModules
    {
        public byte[] ParseData(byte[] data)
        {
            if (data[0] != 0x88) return null;
            return data.SubArray(1, data.Length - 1);
        }
    }
}
