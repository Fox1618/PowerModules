using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Sinexcel
{
    public sealed class SPackSinexcelSetMode : SPackSinexcelBase
    {
        public SPackSinexcelSetMode(byte group, byte subAddress, SinexcelMode mode) : base(group, subAddress, (ushort)CommandCodesSinexcel.SetVoltageMode)
        {
            Mode = mode;
            SetInt32Value((int)mode);
        }

        public SinexcelMode Mode { get; }
    }
}
