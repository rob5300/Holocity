using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Adjustment
{
    public abstract class AdjustableValue<T> where T : struct, IComparable
    {
        public T Value {
            get {
                return GetValue();
            }
        }
        protected T _value;

        public readonly ValueAdjuster<T> Adjuster;

        public AdjustableValue(ValueAdjuster<T> adjuster)
        {
            Adjuster = adjuster;
        }

        protected virtual T GetValue()
        {
            return Adjuster.AdjustValue(_value);
        }
    }

    public class AdjustableFloat : AdjustableValue<float>
    {
        public AdjustableFloat(ValueAdjuster<float> adjuster) : base(adjuster)
        {
        }

        protected override float GetValue()
        {
            return Adjuster.AdjustValue(_value);
        }
    }
}
