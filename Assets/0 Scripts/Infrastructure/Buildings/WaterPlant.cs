﻿using CityResources;
using Infrastructure.Tick;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class WaterPlant : Building, Tickable
    {
        public int PowerIncreaseRate = 15;

        private Water _waterResource;

        public WaterPlant()
        {
            PrefabName = "Waterplant";
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            _waterResource = grid.ParentCity.GetResource<Water>();
        }

        public void Tick(float time)
        {
            _waterResource.Add(PowerIncreaseRate * time);
        }
    }
}