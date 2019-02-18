using Infrastructure.Grid;
using System;
using UnityEngine;

namespace Infrastructure.Grid.Entities.Buildings
{
    public abstract class Building : ResourceConductEntity
    {
        public string Name;

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
