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

        protected Action electricityWarningTask;
        protected Action waterWarningTask;

        protected TaskManager.WorldGridTask setResTask;

        protected WorldGridTile _tile;

        public Building()
        {
            ResourcesFolderPath = "Buildings/";

            electricityWarningTask = () => { ElectricityWarning.SetActive(!HasPower); };
            waterWarningTask = () => { WaterWarning.SetActive(!HasWaterSupply); };
        }

        public override void OnWorldGridTileCreated(WorldGridTile tile)
        {
            base.OnWorldGridTileCreated(tile);

            _tile = tile;
            InstantiateWarningIndicators(tile);
        }

        protected void InstantiateWarningIndicators(WorldGridTile tile)
        {
            float boundmax = 0.1f;
            MeshRenderer renderer = tile.Model.GetComponent<MeshRenderer>();
            if(renderer == null)
            {
                MeshRenderer[] renderers = tile.Model.GetComponentsInChildren<MeshRenderer>();
                foreach(MeshRenderer rend in renderers)
                {
                    Vector3 bounds = new Vector3(rend.bounds.max.x * rend.transform.lossyScale.x, rend.bounds.max.y * rend.transform.lossyScale.y, rend.bounds.max.z * rend.transform.lossyScale.z);
                    if (bounds.y > boundmax) boundmax = bounds.y;
                    if (bounds.z > boundmax) boundmax = bounds.z;
                }
            }
            else
            {
                boundmax = renderer.bounds.max.y * renderer.transform.lossyScale.y;
                float boundzscaled = renderer.bounds.max.z * renderer.transform.lossyScale.z;
                if (boundzscaled > boundmax) boundmax = boundzscaled;
            }
            
            //if (boundmax < tile.Model.GetComponent<MeshRenderer>().bounds.max.z) boundmax = tile.Model.GetComponent<MeshRenderer>().bounds.max.z;
            Vector3 offset = tile.transform.position + new Vector3(0, boundmax, 0);

            if (ElectricityWarning != null) UnityEngine.Object.Destroy(ElectricityWarning);
            ElectricityWarning = UnityEngine.Object.Instantiate(Game.CurrentSession.Cache.ElectricityWarning);
            ElectricityWarning.transform.position = offset;

            if (WaterWarning != null) UnityEngine.Object.Destroy(WaterWarning);
            WaterWarning = UnityEngine.Object.Instantiate(Game.CurrentSession.Cache.WaterWarning);
            WaterWarning.transform.position = offset;
        }

        public override void OnMoveComplete()
        {
            base.OnMoveComplete();

            InstantiateWarningIndicators(_tile);
        }
    }
}
