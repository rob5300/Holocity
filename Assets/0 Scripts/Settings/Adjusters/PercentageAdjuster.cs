using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Settings.Adjustment
{
    public class PercentageAdjuster : ValueAdjuster<float>
    {
        public float ReferenceValue;
        public float Percentage;

        public PercentageAdjuster(float percentage)
        {
            Percentage = percentage;
        }

        public override float AdjustValue(float value)
        {
            return value + (ReferenceValue * Percentage);
        }

    }
}
