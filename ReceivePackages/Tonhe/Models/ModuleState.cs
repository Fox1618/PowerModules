using Ninject.Planning.Bindings;
using System;
using System.Linq;
using System.Reflection;
using System.Xml.Xsl;

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
            Voltage = BitConverter.ToUInt16(data.SubArray(1, 2), 0);
            Current = BitConverter.ToUInt16(data.SubArray(3, 2), 0);
            StateBits = new StateBitsTonhe(data.SubArray(5, 2));
        }
        public ChargingState State { get; set; }
        public float Voltage { get; set; }
        public float Current { get; set; }
        public StateBitsTonhe StateBits { get; set; }
    }

    public class StateBitsTonhe
    {
        private byte[] _data;
        public StateBitsTonhe(byte[] data)
        {
            _data = data;
            var fb = data[0];
            InputUnderVoltage = Helper.GetBit(fb, 1);
            InputPhaseLoss = Helper.GetBit(fb, 2);
            InputOverVoltage = Helper.GetBit(fb, 3);
            OutputOverVoltage = Helper.GetBit(fb, 4);
            OutputOverCurrent = Helper.GetBit(fb, 5);
            TemperatureHigh = Helper.GetBit(fb, 6);
            FanFault = Helper.GetBit(fb, 7);
            HardwareFault = Helper.GetBit(fb, 8);
            BusException = Helper.GetBit(fb, 9);
            SciCommunicationException = Helper.GetBit(fb, 10);
            DischargeFault = Helper.GetBit(fb, 11);
            PfcShutdownDueException = Helper.GetBit(fb, 12);
            OutputUnderVoltageWarning = Helper.GetBit(fb, 13);
            OutputOverVoltageWarning = Helper.GetBit(fb, 14);
            PowerLimitDueHighTemp = Helper.GetBit(fb, 15);
            ShortCircuitFault = Helper.GetBit(fb, 16);
            var sb = data[1];
            InputOverCurrentFault = Helper.GetBit(sb, 1);
            MainsFrequencyFault = Helper.GetBit(sb, 2);
            MainsImbalanceFault = Helper.GetBit(sb, 3);
            DCTzFault = Helper.GetBit(sb, 4);
            AddressConflict = Helper.GetBit(sb, 5);
            BusDias = Helper.GetBit(sb, 6);
            PhaseExceptionFault = Helper.GetBit(sb, 7);
            BusOverVoltageFault = Helper.GetBit(sb, 8);

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
        public bool IsHaveFault => _data.Any(x => x > 0);
        public string Errors
        {
            get
            {
               return !IsHaveFault? "" : GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType ==
                    typeof(bool) && (bool)p.GetValue(this)).Aggregate("", (accum, current) => accum + ", " + current.Name);
            }
        }
    }
}
