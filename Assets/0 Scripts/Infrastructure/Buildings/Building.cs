using Infrastructure.Grid;
using System;
using UnityEngine;

namespace Infrastructure.Grid.Entities.Buildings
{
    public enum BuildingCategory
    {
        Residential,
        Commercial,
        Resource,
        All
    }

    public abstract class Building : ResourceConductEntity
    {
        public string Name;
        public BuildingCategory category;

        protected GameObject ElectricityWarning;
        protected GameObject WaterWarning;

        private WorldGridTaskManager.WorldGridTask setResTask;

        public Building()
        {
            ResourcesFolderPath = "Buildings/";
        }

        public override void OnWorldGridTileCreated(WorldGridTile tile)
        {
            base.OnWorldGridTileCreated(tile);
        }
    }
}
