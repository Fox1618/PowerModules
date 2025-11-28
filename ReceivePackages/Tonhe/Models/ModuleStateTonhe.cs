using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using Telemetria.Hardware.Charger.Models.Common;
using Telemetria.Hardware.PowerModules.Common;

namespace Telemetria.Hardware.PowerModules.ReceivePackages.Tonhe.Models
{
    public enum ChargingState
    {
        Off = 0x00,
        On = 0x01,
        FaultOff = 0x11
    }

    public class ModuleStateTonhe
    {
        public ModuleStateTonhe(byte[] data) {
            State = (ChargingState)data[0];
            Voltage = (float)BitConverter.ToUInt16(data.SubArray(1, 2).ToArray(), 0) / 10;
            Current = (float)BitConverter.ToUInt16(data.SubArray(3, 2).ToArray(), 0) / 100;
            StateBits = new StateBitsTonhe(data.SubArray(5, 3), State);
        }
        public ChargingState State { get; set; }
        public float Voltage { get; set; }
        public float Current { get; set; }
        public StateBitsTonhe StateBits { get; set; }
    }

    public class StateBitsTonhe : StateBase
    {
        public StateBitsTonhe(byte[] data, ChargingState chargingState) : base(data)
        {
            IsChargingStateFault = chargingState == ChargingState.FaultOff;
            var b1 = data[0];
            InputUnderVoltage = Helper.GetBit(b1, 1);
            InputPhaseLoss = Helper.GetBit(b1, 2);
            InputOverVoltage = Helper.GetBit(b1, 3);
            OutputOverVoltage = Helper.GetBit(b1, 4);
            OutputOverCurrent = Helper.GetBit(b1, 5);
            TemperatureHigh = Helper.GetBit(b1, 6);
            FanFault = Helper.GetBit(b1, 7);
            HardwareFault = Helper.GetBit(b1, 8);

            var b2 = data[1];
            BusException = Helper.GetBit(b2, 1);
            SciCommunicationException = Helper.GetBit(b2, 2);
            DischargeFault = Helper.GetBit(b2, 3);
            PfcShutdownDueException = Helper.GetBit(b2, 4);
            OutputUnderVoltageWarning = Helper.GetBit(b2, 5);
            OutputOverVoltageWarning = Helper.GetBit(b2, 6);
            PowerLimitDueHighTemp = Helper.GetBit(b2, 7);
            ShortCircuitFault = Helper.GetBit(b2, 8);

            var b3 = data[2];
            InputOverCurrentFault = Helper.GetBit(b3, 1);
            MainsFrequencyFault = Helper.GetBit(b3, 2);
            MainsImbalanceFault = Helper.GetBit(b3, 3);
            DCTzFault = Helper.GetBit(b3, 4);
            AddressConflict = Helper.GetBit(b3, 5);
            BusDias = Helper.GetBit(b3, 6);
            PhaseExceptionFault = Helper.GetBit(b3, 7);
            BusOverVoltageFault = Helper.GetBit(b3, 8);

        }
        public bool InputUnderVoltage { get; set; }
        public bool InputPhaseLoss { get; set; }
        public bool InputOverVoltage { get; set; }
        public bool OutputOverVoltage { get; set; }
        public bool OutputOverCurrent { get; set; }
        public bool TemperatureHigh { get; set; }
        public bool FanFault { get; set; }
        public bool HardwareFault { get; set; }
        public bool BusException { get; set; }
        public bool SciCommunicationException { get; set; }
        public bool DischargeFault { get; set; }
        public bool PfcShutdownDueException { get; set; }
        public bool OutputUnderVoltageWarning { get; set; }
        public bool OutputOverVoltageWarning { get; set; }
        public bool PowerLimitDueHighTemp { get; set; }
        public bool ShortCircuitFault { get; set; }
        public bool InputOverCurrentFault { get; set; }
        public bool MainsFrequencyFault { get; set; }
        public bool MainsImbalanceFault { get; set; }
        public bool DCTzFault { get; set; }
        public bool AddressConflict { get; set; }
        public bool BusDias { get; set; }
        public bool PhaseExceptionFault { get; set; }
        public bool BusOverVoltageFault { get; set; }
        public bool IsChargingStateFault { get; set; }
        public override uint Code => BitConverter.ToUInt32(new byte[] {_Data[0], _Data[1], _Data[2], 0}, 0);
    }
}
