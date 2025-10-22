using System;

namespace Telemetria.Hardware.PowerModules.Common
{
    public class StateAttribute : Attribute
    {
        public StateAttribute(bool isNotFailure)
        {
            IsNotFailure = isNotFailure;
        }
        public bool IsNotFailure { get; set; }
    }
}