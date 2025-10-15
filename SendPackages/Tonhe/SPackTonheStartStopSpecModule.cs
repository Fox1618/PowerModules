using System;
using System.Collections.Generic;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe;

namespace Telemetria.Hardware.PowerModules.SendPackages.Tonhe
{
    public class SPackTonheStartStopSpecModule : SPackTonheBase
    {
        public SPackTonheStartStopSpecModule()
        {
            CommandType = (byte) CommandCodesTonhe.StartStopSpecModule;
            NeedAnswer = true;
        }
        public bool IsStart { get; set; }
        public byte Mode { get; set; } = 0;
        public float Voltage { get; set; }
        public float Current { get; set; }
        public byte[] Standby { get; set; } = new byte[2];
        public override byte[] Data
        {
            get
            {
                var data = new List<byte>
                {
                    [0] = IsStart ? Constants.StartCode : Constants.StopCode
                };
                data.Add(Mode);
                data.AddRange(BitConverter.GetBytes((ushort)Voltage*10));
                data.AddRange(BitConverter.GetBytes((ushort)Current * 10));
                data.AddRange(Standby);
                return data.ToArray();
            }
        }
    }
}
