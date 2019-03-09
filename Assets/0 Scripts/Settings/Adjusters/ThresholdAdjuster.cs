using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Settings.Adjustment
{
    /// <summary>
    /// Adjusts a float value by a custom percentage when the input modifier value reaches a threshold.
    /// </summary>
    public class ThresholdAdjusterFloat : ValueAdjuster<float>
    {
        public float PercentageModifier;
        public float InputValue;
        public float AdjustThreshold;

        private AdjustableFloat _adjVal;

        public override float AdjustValue(float value)
        {
            if(InputValue >= AdjustThreshold)
            {
                return value * PercentageModifier;
            }

            return value;
        }

        public override void SetAdjustableValue(AdjustableValue<float> val)
        {
            _adjVal = (AdjustableFloat)val;
        }
    }

    public class ThresholdAdjusterInt : ValueAdjuster<int>
    {
        public float PercentageModifier;
        public float InputValue;
        public int AdjustThreshold;

        private AdjustableInt _adjVal;

        public override int AdjustValue(int value)
        {
            if (InputValue >= AdjustThreshold)
            {
                return Mathf.CeilToInt(value * PercentageModifier);
            }

            return value;
        }

        public override void SetAdjustableValue(AdjustableValue<int> val)
        {
            _adjVal = (AdjustableInt)val;
        }
    }
}
