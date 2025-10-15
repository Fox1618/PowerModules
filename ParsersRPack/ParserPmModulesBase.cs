using Hardware.DeviceSending.Interfaces;
using Telemetria.Hardware.PowerModules.Interfaces;

namespace Telemetria.Hardware.PowerModules.ParsersRPack
{
    public abstract class ParserPmModulesBase<T> : IParserPmModulesRPackage<T> where T : class, IRPackBase
    {
        protected IParserOuterPackPmModules _ParserOuterPack;
        
        protected ParserPmModulesBase(IParserOuterPackPmModules parserOuterPack)
        {
            _ParserOuterPack = parserOuterPack;
        }
        public virtual T GetReceivePack(byte[] data, ISPackBase packS = null)
        {
            return null;
        }
    }
}
