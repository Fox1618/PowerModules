using System.Linq;
using Hardware.DeviceSending;

namespace Telemetria.Hardware.PowerModules.SendPackages
{
    public class SPackPmModulesOuterUsr<T> : SPackOuter<T> where T : SPackBase
    {
        //todo 
        private const byte _frame_information_ = 0x88;
        public SPackPmModulesOuterUsr(SPackPModulesBase packInner) : base(packInner)
        {
            PackInner = packInner as T;
        }
        public override byte[] ToByte()
        {
            var data = PackInner.ToByte();
            data.ToList().Insert(0, _frame_information_);
            return data;
        }

        public override int CountRepeat
        {
            get => PackInner.CountRepeat;
            set => PackInner.CountRepeat = value;
        }
    }
    public class SPackOuter<T> : SPackBase where T : SPackBase
    {
        //todo 
        public SPackOuter(SPackBase packInner)
        {
            PackInner = (T)packInner;
        }
        public T PackInner { get; set; }
        public override byte[] ToByte()
        {
            return PackInner.ToByte();
        }
        public override int CountRepeat
        {
            get => PackInner.CountRepeat;
            set => PackInner.CountRepeat = value;
        }

        public override string Name => PackInner.Name;
        public override bool NeedAnswer
        {
            get => PackInner.NeedAnswer;
            set => PackInner.NeedAnswer = value;
        }
        public override object Param
        {
            get => PackInner.Param;
            set => PackInner.Param = value;
        }

        public override int SleepAfter
        {
            get => PackInner.SleepAfter;
            set => PackInner.SleepAfter = value;
        }
    }
}
