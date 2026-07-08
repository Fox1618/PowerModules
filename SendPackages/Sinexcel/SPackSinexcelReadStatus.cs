using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Sinexcel
{
    public sealed class SPackSinexcelReadStatus : SPackSinexcelBase
    {
        public SPackSinexcelReadStatus(byte group, byte subAddress)
            : base(group, subAddress, (ushort)CommandCodesSinexcel.QueryComprehensive)
        {
            SetInt32Value(0);
        }
    }
}