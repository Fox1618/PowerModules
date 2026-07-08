using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Sinexcel
{
    public class RPackSinexcelReadStatus : RPackSinexcelBase
    {
        public RPackSinexcelReadStatus(byte[] data) : base(data)
        {
            switch (MessageId)
            {
                case 0x0202: 
                    ushort dcStatus = (ushort)((Value >> 16) & 0xFFFF);
                    IsOperating = dcStatus == 2; 
                    break;

                case 0x0203:
                    ushort dcFault1 = (ushort)(Value & 0xFFFF);
                    ushort dcFault2 = (ushort)((Value >> 16) & 0xFFFF);

                    HasDcFault = dcFault1 != 0 || dcFault2 != 0;
                    OutputOvervoltage = (dcFault1 & 0x0001) != 0;
                    OverTemperature = (dcFault1 & 0x0002) != 0;
                    FanFault = (dcFault1 & 0x0010) != 0;
                    OutputUndervoltage = (dcFault1 & 0x1000) != 0;
                    break;

                case 0x0204:
                    ushort acFault1 = (ushort)(Value & 0xFFFF);
                    ushort acFault2 = (ushort)((Value >> 16) & 0xFFFF);

                    HasAcFault = acFault1 != 0 || acFault2 != 0;
                    AcInputPhaseLoss = (acFault1 & 0x0001) != 0;
                    AcVoltageOvervoltage = (acFault2 & 0x0001) != 0;
                    AcVoltageUndervoltage = (acFault2 & 0x0002) != 0;
                    break;
            }
        }

        public bool IsOperating { get; set; }
        public bool HasDcFault { get; set; }
        public bool HasAcFault { get; set; }

        //DC
        public bool OutputOvervoltage { get; set; }
        public bool OverTemperature { get; set; }
        public bool FanFault { get; set; }
        public bool OutputUndervoltage { get; set; }

        //AC
        public bool AcInputPhaseLoss { get; set; }
        public bool AcVoltageOvervoltage { get; set; }
        public bool AcVoltageUndervoltage { get; set; }
    }
}