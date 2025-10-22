using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Uugreen.Models
{
    public enum OutputModeUugreen
    {
        VoltageLoop,
        Reserved1,
        CurrentLoop,
        Reserved2
    }
    public class StatusFlagsUugreen : StateBase
    {
       
        public StatusFlagsUugreen(byte[] data): base(data)
        {
            var b1 = data[0];
            AcOverVoltage = Helper.GetBit(b1, 1);
            AcUnderVoltage = Helper.GetBit(b1, 2);
            DisconnectFromAc = Helper.GetBit(b1, 3);
            PfcBusOverVoltage = Helper.GetBit(b1, 4);
            PfcBusUnderVoltage = Helper.GetBit(b1, 5);
            PfcBusUnbalance = Helper.GetBit(b1, 6);
            DcOutputOverVoltage = Helper.GetBit(b1, 7);
            DcOverVoltageShutdown = Helper.GetBit(b1, 8);

            var b2 = data[1];
            DcOutputUnderVoltage = Helper.GetBit(b2, 1);
            FanStopsOperating = Helper.GetBit(b2, 2);
            Reserved1 = Helper.GetBit(b2, 3);
            FanDrivenCircuitDamaged = Helper.GetBit(b2, 4);
            OverAmbientTemp = Helper.GetBit(b2, 5);
            LowAmbientTemp = Helper.GetBit(b2, 6);
            PfcOverTempProtection1 = Helper.GetBit(b2, 7);
            OutputRelayFailure = Helper.GetBit(b2, 8);

            var b3 = data[2];
            DcOverTempProtection1 = Helper.GetBit(b3, 1);
            DcOverTempProtection2 = Helper.GetBit(b3, 2);
            CommunicationFailurePfcDcDc = Helper.GetBit(b3, 3);
            DcDcNotGetPfcOkSignal = Helper.GetBit(b3, 4);
            PfcFailure = Helper.GetBit(b3, 5);
            DcDcFailure = Helper.GetBit(b3, 6);
            Reserved2 = Helper.GetBit(b3, 7);
            Reserved3 = Helper.GetBit(b3, 8);

            var b4 = data[3];
            PfcState = Helper.GetBit(b4, 1);
            ModuleStatus = Helper.GetBit(b4, 2);
            OutputMode = (OutputModeUugreen) Helper.GetBits(b4, 3, 2);
            DcOutputVoltageUnbalance = Helper.GetBit(b4, 5);
            GetSameSn = Helper.GetBit(b4, 6);
            Reserved4 = Helper.GetBit(b4, 7);
            BleederNotWork = Helper.GetBit(b4, 8);
        }
        public bool AcOverVoltage { get; set; }
        public bool AcUnderVoltage { get; set; }
        public bool DisconnectFromAc { get; set; }
        public bool PfcBusOverVoltage { get; set; }
        public bool PfcBusUnderVoltage { get; set; }
        public bool PfcBusUnbalance { get; set; }
        public bool DcOutputOverVoltage { get; set; }
        public bool DcOverVoltageShutdown { get; set; }
        public bool DcOutputUnderVoltage { get; set; }
        public bool FanStopsOperating { get; set; }
        public bool Reserved1 { get; set; }
        public bool FanDrivenCircuitDamaged { get; set; }
        public bool OverAmbientTemp { get; set; }
        public bool LowAmbientTemp { get; set; }
        public bool PfcOverTempProtection1 { get; set; }
        public bool OutputRelayFailure { get; set; }
        public bool DcOverTempProtection1 { get; set; }
        public bool DcOverTempProtection2 { get; set; }
        public bool CommunicationFailurePfcDcDc { get; set; }
        public bool DcDcNotGetPfcOkSignal { get; set; }
        public bool PfcFailure { get; set; }
        public bool DcDcFailure { get; set; }
        public bool Reserved2 { get; set; }
        public bool Reserved3 { get; set; }
        public bool PfcState { get; set; }
        public bool ModuleStatus { get; set; }
        public OutputModeUugreen OutputMode { get; set; }
        public bool DcOutputVoltageUnbalance { get; set; }
        public bool GetSameSn { get; set; }
        public bool Reserved4 { get; set; }
        public bool BleederNotWork { get; set; }
    }
}
