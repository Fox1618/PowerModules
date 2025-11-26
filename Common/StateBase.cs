using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Telemetria.Hardware.PowerModules.Common
{
    public class StateBase
    {
        protected readonly byte[] _Data;

        public StateBase(byte[] data)
        {
            _Data = data;
        }
        [State(true)]
        public bool IsHaveFault => _PropertiesFailures.Any();

        private List<PropertyInfo> _propertiesFailuresL;

        protected List<PropertyInfo> _PropertiesFailures
        {
            get
            {
                if (_propertiesFailuresL != null) return _propertiesFailuresL;
                _propertiesFailuresL = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p =>
                    p.GetCustomAttribute<StateAttribute>()?.IsNotFailure != true &&
                    p.PropertyType ==
                    typeof(bool) && (bool)p.GetValue(this)).ToList();
                return _propertiesFailuresL;
            }
        }
        [State(true)]
        public string Errors
        {
            get
            {
                return !IsHaveFault ? "" : string.Join(",", _PropertiesFailures.Select(x => x.Name));
            }
        }

        public virtual uint Code => BitConverter.ToUInt32(_Data, 0);
    }
}
