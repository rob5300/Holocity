using System.Collections.Generic;

namespace Infrastructure
{
    /// <summary>
    /// Manages adjusting values based on watched values. Used to adjust diffuclty of the game.
    /// </summary>
    public class DynamicDifficultyAdjuster
    {
        public List<AdjustmentRule> AdjustmentRules;

        public DynamicDifficultyAdjuster()
        {
            AdjustmentRules = new List<AdjustmentRule>();
        }
    }

    public class AdjustmentRule
    {
        public string ID;
        public AdjustmentValue Value;
    }

    public class AdjustmentValue
    {
        public object ValueObject;
    }
}
