namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen.Models
{
    public enum HiLoModeUugreen
    {
        NoSelection = 0,
        HiMode = 2,
        Low = 3
    }

    public enum OnOffModeUugreen
    {
        TurnOnP = 0,
        ShutDownDcNotPfc = 1,
        TurnOnDcPfc = 2,
        ShutDownDcPfc = 3

    }
    public class HiLoModeOnOffUugreen
    {
        public HiLoModeOnOffUugreen(byte data)
        {
            HiLoMode = (HiLoModeUugreen)Helper.GetBits(data, 6, 2);
            OnOffMode = (OnOffModeUugreen)Helper.GetBits(data, 1, 2);
        }
        public HiLoModeUugreen HiLoMode { get; set; }
        public OnOffModeUugreen OnOffMode { get; set; }

        public byte ToByte()
        {
            return (byte)(Helper.GetBits((byte) HiLoMode, 1, 2) << 6 | Helper.GetBits((byte) OnOffMode, 1, 2));
        }
    }
}
