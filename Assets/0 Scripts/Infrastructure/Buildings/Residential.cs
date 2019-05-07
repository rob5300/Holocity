
using Infrastructure.Residents;
using Infrastructure.Tick;
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Residential : Building
    {
        public List<Resident> Residents { get; private set; }
        public int ResidentCapacity = 1;
        public int VacantSlots { get
            {
                int count = ResidentCapacity - Residents.Count;
                if (count < 0) count = 0;
                return count;
            } }

        public Residential()
        {
            Residents = new List<Resident>();
            category = BuildingCategory.Residential;
        }

        public virtual void AddResident(Resident res)
        {
            if (VacantSlots != 0)
            {
                Residents.Add(res);
            }
            if(res is ITickable)
            {
                res.Home = this;
                //Add this new resident to the owning gridsystem reference list.
                GridSystem gs = ParentTile.ParentGridSystem;
                if(!gs.Residents.Contains(res)) gs.Residents.Add(res);
            }
        }

        public virtual void RemoveResident(Resident res)
        {
            if(Residents.Contains(res))
            {
                ParentTile.ParentGridSystem.ParentCity.ProcessHomelessResident(res);
                ParentTile.ParentGridSystem.ParentCity.ProcessResidentialAsVacant(this);
                Residents.Remove(res);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            foreach(Resident r in Residents)
            {
                RemoveResident(r);
            }
        }
    }
}
