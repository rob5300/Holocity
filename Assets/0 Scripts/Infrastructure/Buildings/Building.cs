using Infrastructure.Grid;
using System;
using UnityEngine;

namespace Infrastructure.Grid.Entities.Buildings
{
    public abstract class Building : TileEntity
    {
        public static string BuildingResourcesFolderPath = "Buildings/";

        public string Name;
        public GameObject BuildingPrefab {
            get {
                if (_buildingPrefab == null) _buildingPrefab = GetBuildinbPrefab(BuildingPrefabPath);
                return _buildingPrefab;
            }
        }

        public string BuildingPrefabPath;
        [NonSerialized]
        private GameObject _buildingPrefab;

        public Building()
        {
            
        }

        public virtual bool QueryPlace(Grid.GridTile tile)
        {
            return true;
        }

        public virtual void OnWorldGridTileCreated(WorldGridTile tile)
        {

        }

        private static GameObject GetBuildinbPrefab(string path) {
            return Resources.Load<GameObject>(BuildingResourcesFolderPath + path);
        }
    }
}
