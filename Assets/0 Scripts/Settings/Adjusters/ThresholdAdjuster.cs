using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Settings.Adjustment
{
    public enum ThresholdCheckMode { Larger, Smaller };
    public enum ThresholdAdjustmentMode { Multiply, Add, Divide, Subtract };

    /// <summary>
    /// Adjusts a float value by a custom percentage when the input modifier value reaches a threshold.
    /// </summary>
    public class ThresholdAdjusterFloat : ValueAdjuster<float>
    {
        public float Modifier;
        public float InputValue;
        public float AdjustThreshold;
        public ThresholdCheckMode ThresholdAdjustMode;
        public ThresholdAdjustmentMode AdjustmentMode;

        private AdjustableFloat _adjVal;

        public ThresholdAdjusterFloat(ThresholdCheckMode thresholdAdjustMode, ThresholdAdjustmentMode adjustmentMode, float Modif, float adjThreshold)
        {
            ThresholdAdjustMode = thresholdAdjustMode;
            AdjustmentMode = adjustmentMode;
            Modifier = Modif;
            AdjustThreshold = adjThreshold;
        }

        public override float AdjustValue(float value)
        {
            if (ThresholdAdjustMode == ThresholdCheckMode.Larger)
            {
                if (InputValue >= AdjustThreshold)
                {
                    return AdjustviaMode(value);
                } 
            }
            else if(ThresholdAdjustMode == ThresholdCheckMode.Smaller)
            {
                if (InputValue <= AdjustThreshold)
                {
                    return AdjustviaMode(value);
                }
            }

            return value;
        }

        private float AdjustviaMode(float toAdjust)
        {
            switch (AdjustmentMode)
            {
                case ThresholdAdjustmentMode.Add:
                    return toAdjust + Modifier;
                case ThresholdAdjustmentMode.Subtract:
                    return toAdjust - Modifier;
                case ThresholdAdjustmentMode.Multiply:
                    return toAdjust * Modifier;
                case ThresholdAdjustmentMode.Divide:
                    return toAdjust / Modifier;
                default:
                    return toAdjust;
            }
        }

        public override void SetParentAdjustableValue(AdjustableValue<float> val)
        {
            _adjVal = (AdjustableFloat)val;
        }
    }

    public class ThresholdAdjusterInt : ValueAdjuster<int>
    {
        public float Modifier;
        public float InputValue;
        public int AdjustThreshold;
        public ThresholdCheckMode ThresholdAdjustMode;
        public ThresholdAdjustmentMode AdjustmentMode;

        private AdjustableInt _adjVal;

        public ThresholdAdjusterInt(ThresholdCheckMode thresholdAdjustMode, ThresholdAdjustmentMode adjustmentMode, float Modif, int adjThreshold)
        {
            ThresholdAdjustMode = thresholdAdjustMode;
            AdjustmentMode = adjustmentMode;
            Modifier = Modif;
            AdjustThreshold = adjThreshold;
        }

        public override int AdjustValue(int value)
        {
            if (ThresholdAdjustMode == ThresholdCheckMode.Larger)
            {
                if (InputValue >= AdjustThreshold)
                {
                    return AdjustviaMode(value);
                }
            }
            else if (ThresholdAdjustMode == ThresholdCheckMode.Smaller)
            {
                if (InputValue <= AdjustThreshold)
                {
                    return AdjustviaMode(value);
                }
            }

            return value;
        }

        private int AdjustviaMode(int toAdjust)
        {
            float result;
            switch (AdjustmentMode)
            {
                case ThresholdAdjustmentMode.Add:
                    result = toAdjust + Modifier;
                    break;
                case ThresholdAdjustmentMode.Subtract:
                    result = toAdjust - Modifier;
                    break;
                case ThresholdAdjustmentMode.Multiply:
                    result = toAdjust * Modifier;
                    break;
                case ThresholdAdjustmentMode.Divide:
                    result = toAdjust / Modifier;
                    break;
                default:
                    return toAdjust;
            }
            return Mathf.CeilToInt(result);
        }

        public override void SetParentAdjustableValue(AdjustableValue<int> val)
        {
            _adjVal = (AdjustableInt)val;
        }
    }
}
