using Settings.Adjustment;
using System;

namespace Settings {

    [Serializable]
    public enum TimePeriod
    {
        Medieval, Modern, Futuristic
    }

    [Serializable]
    public class GameSettings
    {
        #region Constants
        public const uint StartingMoney = 5000;
        public const int StartingResidentialDemand = 20;
        public const float StartingResidentialDemandIncreaseRate = 0.2f;
        public const float ResidentTimeWithLowHappiness = 20f;
        #endregion

        /// <summary>
        /// The demand for residents to move in. If there is avaliable housing found or a new house built, this will reduce to fill the buildings.
        /// </summary>
        public int ResidentialDemand = StartingResidentialDemand;
        public float ResidentialDemandAcculimative = 0;
        public uint Funds;
        public TimePeriod CurrentTimePeriod;
        

        [NonSerialized]
        private ThresholdAdjusterFloat ResidentialDemandAdj = new ThresholdAdjusterFloat(ThresholdCheckMode.Larger, ThresholdAdjustmentMode.Multiply, 0.5f, 0.15f);
        [NonSerialized]
        public AdjustableFloat ResidentialDemandIncreaseRate;
        [NonSerialized]
        public AdjustableFloat BaseSalary;
        [NonSerialized]
        private PercentageAdjuster salaryAdj = new PercentageAdjuster(0.25f);

        [NonSerialized]
        private ThresholdAdjusterFloat commercialAdjuster = new ThresholdAdjusterFloat(ThresholdCheckMode.Smaller, ThresholdAdjustmentMode.Multiply, 1.25f, 1000);
        [NonSerialized]
        public AdjustableFloat CommercialTaxRateModifier;
        

        public GameSettings()
        {
            ResidentialDemandIncreaseRate = new AdjustableFloat(ResidentialDemandAdj);
            ResidentialDemandIncreaseRate.SetValue(StartingResidentialDemandIncreaseRate);

            BaseSalary = new AdjustableFloat(salaryAdj, 5);
            CommercialTaxRateModifier = new AdjustableFloat(commercialAdjuster, 0.05f);
            Funds = StartingMoney;
        }

        public GameSettings(TimePeriod time) : this()
        {
            CurrentTimePeriod = time;
        }

        public void UpdateValues(Session s, float tickTime)
        {
            //Update the values for the residential increase amount adjuster
            //Input the percent of the current resident demand vs the total residents we have
            if(s.City.Residents.Count != 0 && ResidentialDemand != 0) ResidentialDemandAdj.InputValue = s.City.Residents.Count > 0 ? s.City.Residents.Count / ResidentialDemand : 0;

            //Increase residential demand and keep track of the decimal values.
            //Floor to only increase when the rate goes to an int again.
            ResidentialDemandAcculimative += ResidentialDemandIncreaseRate.Value * tickTime;
            ResidentialDemand = (int)Math.Floor(ResidentialDemandAcculimative);

           //Update base salary input value
           salaryAdj.ReferenceValue = s.City.Residents.Count;

            commercialAdjuster.InputValue = Funds;
        }
    }
}
