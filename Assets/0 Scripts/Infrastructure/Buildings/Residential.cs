
using Infrastructure.Residents;
using Infrastructure.Tick;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Residential : Building
    {
        public Resident Resident { get; private set; }
        public bool IsVacant { get { return Resident == null; } }
        public int VacantSlots { get; private set; }
        /// <summary>
        /// If this building has power;
        /// </summary>
        public bool HasPower = false;
        /// <summary>
        /// If this building has a water supply.
        /// </summary>
        public bool HasWaterSupply = false;
        private GridTile parent;

        protected WorldGridTaskManager.WorldGridTask electricityWarningTask;
        protected WorldGridTaskManager.WorldGridTask waterWarningTask;

        public Residential()
        {
            electricityWarningTask = (grid) => { ElectricityWarning.SetActive(!HasPower); };
            waterWarningTask = (grid) => { WaterWarning.SetActive(!HasWaterSupply); };
            VacantSlots = 1;
        }

        public virtual void SetResident(Resident res)
        {
            if (Resident == null)
            {
                Resident = res;
                VacantSlots = 0;
            }
            if(res is Tickable)
            {
                res.Home = this;
                //Add this new resident to the owning gridsystem reference list.
                GridSystem gs = ParentTile.ParentGridSystem;
                if(!gs.Residents.Contains(res)) gs.Residents.Add(res);
            }
        }

        public virtual void RemoveResident(Resident res)
        {
            if(Resident == res)
            {
                ParentTile.ParentGridSystem.ParentCity.ProcessHomelessResident(Resident);
                Resident = null;
            }
        }
    }
}
