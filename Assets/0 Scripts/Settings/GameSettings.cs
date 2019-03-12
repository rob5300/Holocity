using Settings.Adjustment;
using System.Collections.Generic;

namespace Settings {
    public class GameSettings
    {
        #region Constants
        public const uint StartingMoney = 50000;
        public const int StartingResidentialDemand = 20;
        public const float StartingResidentialDemandIncreaseRate = 0.05f;
        public const float ResidentTimeWithLowHappiness = 20f;
        #endregion

        /// <summary>
        /// The demand for residents to move in. If there is avaliable housing found or a new house built, this will reduce to fill the buildings.
        /// </summary>
        public int ResidentialDemand = StartingResidentialDemand;
        public AdjustableFloat ResidentialDemandIncreaseRate = new AdjustableFloat(new ThresholdAdjusterFloat(ThresholdCheckMode.Larger, ThresholdAdjustmentMode.Multiply , 0.5f, 0.15f));
        //public AdjustableFloat ResidentTimeWithLowHappiness = new AdjustableFloat(new ThresholdAdjusterFloat(ThresholdCheckMode.Larger, ThresholdAdjustmentMode.Multiply, 0.5f, 0.15f));

        public GameSettings()
        {
            ResidentialDemandIncreaseRate.SetValue(StartingResidentialDemandIncreaseRate);
            //ResidentTimeWithLowHappiness.SetValue(StartResidentTimeWithLowHappiness);
        }
    }
}
