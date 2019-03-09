using Settings.Adjustment;
using System.Collections.Generic;

namespace Settings {
    public class GameSettings
    {
        #region Constants
        public const uint StartingMoney = 50000;
        public const int StartingResidentialDemand = 20;
        #endregion

        /// <summary>
        /// The demand for residents to move in. If there is avaliable housing found or a new house built, this will reduce to fill the buildings.
        /// </summary>
        public AdjustableInt ResidentialDemand = new AdjustableInt(new ThresholdAdjusterInt());
        public AdjustableFloat ResidentialDemandIncreaseRate = new AdjustableFloat(new ThresholdAdjusterFloat());
    }
}
