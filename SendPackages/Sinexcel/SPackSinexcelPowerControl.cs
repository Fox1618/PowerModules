using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Sinexcel
{
    public sealed class SPackSinexcelPowerControl : SPackSinexcelBase
    {
        public SPackSinexcelPowerControl(byte group, byte subAddress, bool isStart)
            : base(
                group,
                subAddress,
                (ushort)(subAddress >= 32 ? 0x0232 : 0x0230))
        {
            IsStart = isStart;

            int bitPosition = subAddress >= 32 ? (subAddress - 32) : subAddress;

            uint mask = 0;
            if (isStart)
            {
                mask |= (1u << bitPosition);
            }

            SetBitMask(mask);
        }

        public bool IsStart { get; }
    }
}
