namespace Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe
{
    public class RPackTonheConfirmStartStop : RPackTonheBase
    {
        public RPackTonheConfirmStartStop(byte[] data) : base(data)
        {
            IsReceived = Data[0] == 1;
            Standby = Data.SubArray(1, 7);
        }
        public bool IsReceived { get; set; }
        public byte[] Standby { get; set; }
    }
}
