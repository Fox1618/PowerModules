using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.SendPackages.Tonhe
{
    public class SPackTonheStartStop : SPackTonheBase
    {
        public SPackTonheStartStop(byte? address = null, byte addressMultiple = 0)
        {
            if (address != null) {
                (AddressMultiple, Addresses) = _getAddress(address.Value);
            }
            if (address == null)
                AddressMultiple = addressMultiple;

            CommandType = (byte) CommandCodesTonhe.StartStop;
            ModuleAddress = byte.MaxValue;
            Priority = 2;
        }
        public byte[] Addresses { get; set; } = { byte.MaxValue, byte.MaxValue, byte.MaxValue};
        public byte AddressMultiple { get; set; }
        public byte[] Standby { get; set; } = new byte[3];
        public bool IsStart { get; set; }

        public override byte[] Data
        {
            get
            {
                var data = new byte[8];
                for (var i = 0; i < Addresses.Length; i++)
                {
                    data[i] = Addresses[i];       
                }
                data[3] = IsStart? Constants.StartCode : Constants.StopCode;
                data[4] = AddressMultiple;
                for (var i = 0; i < Standby.Length; i++)
                {
                    data[5 + i] = Standby[i];
                }
                return data;
            }
        }
    }
}
