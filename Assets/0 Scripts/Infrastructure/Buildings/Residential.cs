﻿
using Infrastructure.Residents;
using Infrastructure.Tick;
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Residential : Building
    {
        public Resident Resident { get; private set; }
        public List<Resident> Residents { get; private set; }
        public bool IsVacant { get { return Resident == null; } }
        public int VacantSlots { get; private set; }

        public Residential()
        {
            VacantSlots = 1;
            Residents = new List<Resident>();
            category = BuildingCategory.Residential;
        }

        public virtual void SetResident(Resident res)
        {
            if (Resident == null)
            {
                Resident = res;
                VacantSlots = 0;
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
            if(Resident == res)
            {
                ParentTile.ParentGridSystem.ParentCity.ProcessHomelessResident(Resident);
                ParentTile.ParentGridSystem.ParentCity.ProcessResidentialAsVacant(this);
                Resident = null;
                Game.CurrentSession.TaskManager.Tasks.Enqueue(() => { UnityEngine.Debug.Log("Resident moved out from " + Name); });
            }
        }
    }
}
