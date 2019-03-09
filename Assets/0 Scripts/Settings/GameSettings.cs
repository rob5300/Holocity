using Settings.Adjustment;
using System.Collections.Generic;

namespace Settings {
    public class GameSettings
    {
        #region Constants
        public const uint StartingMoney = 50000;

        #endregion

        public AdjustableFloat ResidentialDemand = new AdjustableFloat(new ThresholdAdjuster());
    }
}
