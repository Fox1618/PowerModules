using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Sinexcel
{
    public sealed class SPackSinexcelQueryComprehensive : SPackSinexcelBase
    {
        public SPackSinexcelQueryComprehensive(byte group, byte subAddress) : base(group, subAddress, (ushort)CommandCodesSinexcel.QueryComprehensive, (byte)CommandCodesSinexcel.MessageTypeQuery)
        {
        }
    }
}
