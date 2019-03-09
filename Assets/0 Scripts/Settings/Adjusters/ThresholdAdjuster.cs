using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Adjustment
{
    /// <summary>
    /// Adjusts a float value by a custom percentage when the input modifier value reaches a threshold.
    /// </summary>
    public class ThresholdAdjuster : ValueAdjuster<float>
    {
        private readonly float _percentageModifier;
        private readonly float _adjustThreshold;
        private float _inputValue;

        public float AdjustThreshold { get {
                return _adjustThreshold;
            } }

        public ThresholdAdjuster()
        {
            
        }

        public override float AdjustValue(float value)
        {
            if(_inputValue >= _adjustThreshold)
            {
                return value * _percentageModifier;
            }

            return value;
        }

    }
}
