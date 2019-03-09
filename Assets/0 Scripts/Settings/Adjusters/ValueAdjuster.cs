using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Adjustment
{
    public abstract class ValueAdjuster<T> where T : struct, IComparable
    {
        public abstract T AdjustValue(T value);

        public virtual void SetAdjustableValue(AdjustableValue<T> val)
        {

        }
    }
}
