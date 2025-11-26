using Hardware.DeviceSending.Interfaces;
using Ninject;
using Ninject.Modules;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ParsersRPack;
using Telemetria.Hardware.PowerModules.Senders;
using Telemetria.Hardware.TcpDevices;

namespace Telemetria.Hardware.PowerModules
{
    public class PowerModulesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IParserOuterPackPmModules>().To<ParserOuterPackPmModules>().WhenInjectedExactlyInto(typeof(ParserTonheRPackage<IRPackPmModulesBase>));
            Bind<IParserOuterPackPmModules>().To<ParserOuterPackPmModules>().WhenInjectedExactlyInto(typeof(ParserUugreenRPackage<IRPackPmModulesBase>));
            Bind<IParserPmModulesRPackage<IRPackPmModulesBase>>().To<ParserTonheRPackage<IRPackPmModulesBase>>().WhenInjectedExactlyInto(typeof(TonhePmSender<IRPackPmModulesBase>)).InSingletonScope();
            Bind<IParserPmModulesRPackage<IRPackPmModulesBase>>().To<ParserUugreenRPackage<IRPackPmModulesBase>>().WhenInjectedExactlyInto(typeof(UugreenPmSender<IRPackPmModulesBase>)).InSingletonScope();
            Bind<IDeviceSending>().ToMethod(x => x.Kernel.Get<TcpDevice>()).WhenInjectedExactlyInto(typeof(TonhePmSender<IRPackPmModulesBase>), typeof(UugreenPmSender<IRPackPmModulesBase>));
            Bind<UugreenPmSender<IRPackPmModulesBase>>().ToSelf();
            Bind<TonhePmSender<IRPackPmModulesBase>>().ToSelf();
        }
    }
}
