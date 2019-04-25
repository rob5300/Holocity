using Settings.Adjustment;
using System;

namespace Settings {
    [Serializable]
    public class GameSettings
    {
        #region Constants
        public const uint StartingMoney = 5000;
        public const int StartingResidentialDemand = 20;
        public const float StartingResidentialDemandIncreaseRate = 0.05f;
        public const float ResidentTimeWithLowHappiness = 20f;
        #endregion

        /// <summary>
        /// The demand for residents to move in. If there is avaliable housing found or a new house built, this will reduce to fill the buildings.
        /// </summary>
        public int ResidentialDemand = StartingResidentialDemand;
        public uint Funds;

        [NonSerialized]
        private ThresholdAdjusterFloat ResidentialDemandAdj = new ThresholdAdjusterFloat(ThresholdCheckMode.Larger, ThresholdAdjustmentMode.Multiply, 0.5f, 0.15f);
        [NonSerialized]
        public AdjustableFloat ResidentialDemandIncreaseRate;
        [NonSerialized]
        public AdjustableFloat BaseSalary;
        [NonSerialized]
        private PercentageAdjuster salaryAdj = new PercentageAdjuster(0.25f);

        public GameSettings()
        {
            ResidentialDemandIncreaseRate = new AdjustableFloat(ResidentialDemandAdj);
            ResidentialDemandIncreaseRate.SetValue(StartingResidentialDemandIncreaseRate);

            BaseSalary = new AdjustableFloat(salaryAdj, 5);
            Funds = StartingMoney;
        }

        public void UpdateValues(Session s, float tickTime)
        {
            //Update the values for the residential increase amount adjuster
            //Input the percent of the current resident demand vs the total residents we have
            ResidentialDemandAdj.InputValue = s.City.Residents.Count > 0 ? s.City.Residents.Count / ResidentialDemand : 0;

            //Update base salary input value
            salaryAdj.ReferenceValue = s.City.Residents.Count;
        }
    }
}
