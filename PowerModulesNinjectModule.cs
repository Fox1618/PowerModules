using Hardware.DeviceSending.Interfaces;
using Ninject.Modules;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ParsersRPack;
using Telemetria.Hardware.PowerModules.ReceivePackages;
using Telemetria.Hardware.PowerModules.Senders;
using Telemetria.Hardware.TcpDevices;

namespace Telemetria.Hardware.PowerModules
{
    public class PowerModulesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IParserOuterPackPmModules>().To<ParserOuterPackPmModules>()
                .WhenInjectedExactlyInto(typeof(ParserTonheRPackage<IRPackPmModulesBase>));
            Bind<IDeviceSending>().To<TcpDevice>().WhenInjectedExactlyInto(typeof(TonhePmSender<IRPackPmModulesBase>)).InSingletonScope();
        }
    }
}
