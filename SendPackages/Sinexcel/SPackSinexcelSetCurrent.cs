using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Sinexcel
{
    public sealed class SPackSinexcelSetCurrent : SPackSinexcelBase
    {
        public SPackSinexcelSetCurrent(byte group, byte subAddress, float current) : base(group, subAddress, (ushort)CommandCodesSinexcel.SetOutputCurrent)
        {
            Current = current;
            SetCurrentValue(current);
        }

        public float Current { get; }
    }
}
