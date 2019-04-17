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

        protected GridTile parent;

        /// <summary>
        /// If this building has power;
        /// </summary>
        public bool HasPower = false;
        /// <summary>
        /// If this building has a water supply.
        /// </summary>
        public bool HasWaterSupply = false;

        protected GameObject ElectricityWarning;
        protected GameObject WaterWarning;

        protected WorldGridTaskManager.WorldGridTask electricityWarningTask;
        protected WorldGridTaskManager.WorldGridTask waterWarningTask;

        protected WorldGridTaskManager.WorldGridTask setResTask;

        public Building()
        {
            ResourcesFolderPath = "Buildings/";

            electricityWarningTask = (grid) => { ElectricityWarning.SetActive(!HasPower); };
            waterWarningTask = (grid) => { WaterWarning.SetActive(!HasWaterSupply); };
        }

        public override void OnWorldGridTileCreated(WorldGridTile tile)
        {
            base.OnWorldGridTileCreated(tile);
        }
    }
}
